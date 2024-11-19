using FogOfWarChess.MainCore.MainEngine;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace FogOfWarChess.NetEngine
{
    public class ClientConnection
    {
        private Socket sender;
        private Thread receiveThread;
        private Action<NormalMove> onMoveReceived;
        public static string UserColor { get; private set; }
        public ClientConnection(Action<NormalMove> onMoveReceivedCallback)
        {
            onMoveReceived = onMoveReceivedCallback;
        }

        public bool InitConnect(string serverIP, int port)
        {
            IPAddress ipAddr = IPAddress.Parse(serverIP);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);

            sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(localEndPoint);

            Debug.WriteLine("Connected to server!");

            UserColor = ReceiveInitialMessage(sender);
            Debug.WriteLine($"Assigned user color: {UserColor}");
            
            receiveThread = new Thread(ReceiveData);
            receiveThread.Start();
            if (UserColor == "White")
                return true;
            return false;
        }

        public void EndConnections()
        {
            string message = "END" + "<EOF>";
            byte[] messageSent = Encoding.ASCII.GetBytes(message);
            sender.Send(messageSent);
            Debug.WriteLine("END");
            try
            {
                    receiveThread.Interrupt();
                    receiveThread.Join();

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                    sender = null;

                Debug.WriteLine("Connection ended successfully.");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error ending connection: {e.Message}");
            }
        }

        public void SendMove(NormalMove move)
        {
            string message = NormalMoveToString(move) + "<EOF>";
            byte[] messageSent = Encoding.ASCII.GetBytes(message);
            sender.Send(messageSent);
            Debug.WriteLine("Sent move");
        }
        private void ReceiveData()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int numBytes = sender.Receive(buffer);
                    string receivedMessage = Encoding.ASCII.GetString(buffer, 0, numBytes);

                    if (receivedMessage.EndsWith("<EOF>"))
                    {
                        receivedMessage = receivedMessage.Substring(0, receivedMessage.Length - "<EOF>".Length).Trim();
                        NormalMove move = StringToNormalMove(receivedMessage);
                        onMoveReceived?.Invoke(move);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in receiving data: {0}", e.Message);
            }
        }

        private static string NormalMoveToString(NormalMove move)
        {
            return $"{move.FromPos.Row} {move.FromPos.Column} {move.ToPos.Row} {move.ToPos.Column}";
        }

        private static NormalMove StringToNormalMove(string moveString)
        {
            var parts = moveString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 4 &&
                int.TryParse(parts[0], out int fromRow) &&
                int.TryParse(parts[1], out int fromCol) &&
                int.TryParse(parts[2], out int toRow) &&
                int.TryParse(parts[3], out int toCol))
            {
                return new NormalMove(new Position(fromRow, fromCol), new Position(toRow, toCol));
            }

            throw new FormatException("Invalid move string format.");
        }
        private static string ReceiveInitialMessage(Socket sender)
        {
            byte[] buffer = new byte[1024];
            int numBytes = sender.Receive(buffer);
            return Encoding.ASCII.GetString(buffer, 0, numBytes).Trim();
        }
    }

}
