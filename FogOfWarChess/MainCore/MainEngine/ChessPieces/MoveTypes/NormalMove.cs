using System.Diagnostics;

namespace FogOfWarChess.MainCore.MainEngine
{
    public class NormalMove : Move
    {
        public override MoveType Type => MoveType.Normal;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        public NormalMove(Position from, Position to)
        {
            FromPos = from;
            ToPos = to;
        }

        public override void Execute(ChessBoard board)
        {
            Piece piece = board[FromPos];
            if (piece != null)
            {
                Debug.WriteLine(piece.PieceName);
                board[ToPos] = piece;
                board[FromPos] = null;
                piece.HasMoved = true;
            }
        }
    }

}