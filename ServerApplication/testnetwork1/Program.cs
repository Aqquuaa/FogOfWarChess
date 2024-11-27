using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace Server
{
    class Program
    {
        private static Queue<Socket> waitingClients = new Queue<Socket>();

        static void Main(string[] args)
        {
            ExecuteServer();
        }

        public static void ExecuteServer()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = IPAddress.Loopback; //ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                Console.WriteLine("Waiting for connections...");

                while (true)
                {
                    // Accept a client connection
                    Socket clientSocket = listener.Accept();
                    Console.WriteLine("New client connected!");

                    lock (waitingClients)
                    {
                        waitingClients.Enqueue(clientSocket);
                        // Pair clients if two are waiting
                        if (waitingClients.Count >= 2)
                        {
                            Socket client1 = waitingClients.Dequeue();
                            Socket client2 = waitingClients.Dequeue();
                            Console.WriteLine("Pairing two clients...");

                            // Start a thread to handle the client pair
                            Thread clientPairThread = new Thread(() => HandleClientPair(client1, client2));
                            clientPairThread.Start();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void HandleClientPair(Socket client1, Socket client2)
        {
            Console.WriteLine("Handling communication between two clients...");

            try
            {
                // Generate random boolean
                Random random = new Random();
                bool firstClientBool = random.Next(2) == 0;
                bool secondClientBool = !firstClientBool;

                // Send boolean to both clients
                SendBooleanToClient(client1, firstClientBool);
                SendBooleanToClient(client2, secondClientBool);

                Thread client1ToClient2 = new Thread(() => RelayMessages(client1, client2));
                Thread client2ToClient1 = new Thread(() => RelayMessages(client2, client1));

                client1ToClient2.Start();
                client2ToClient1.Start();

                client1ToClient2.Join();
                client2ToClient1.Join();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in client pair communication: " + e.Message);
            }
            finally
            {
                CloseClient(client1);
                CloseClient(client2);
            }
        }

        private static void SendBooleanToClient(Socket client, bool value)
        {
            string message = null;
            if (value == true)
            {
                message = "White";
            }
            else
            {
                message = "Black";
            }
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            client.Send(messageBytes);
            
            Console.WriteLine($"Sent boolean {value} to client.");
        }

        private static void RelayMessages(Socket sender, Socket receiver)
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int numBytes = sender.Receive(buffer);
                    string message = Encoding.ASCII.GetString(buffer, 0, numBytes);

                    if (message.Contains("END") || message == "")
                    {
                        Console.WriteLine("Ending connection for one client.");
                        break;
                    }

                    Console.WriteLine("Relaying message: {0}", message);
                    receiver.Send(Encoding.ASCII.GetBytes(message));
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Client disconnected: " + e.Message);
            }
        }

        private static void CloseClient(Socket client)
        {
            try
            {
                client.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }
            finally
            {
                client.Close();
            }
        }
    }
}
