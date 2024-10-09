using System.Collections.Generic;
using System.Linq;

namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class Bishop : Piece
{
    public override PieceType Type => PieceType.Bishop;
    public override Color Color { get; }
    private static readonly Direction[] dirs = new Direction[]
    {
        Direction.NorthWest,
        Direction.NorthEast,
        Direction.SouthWest,
        Direction.SouthEast,
    };
    
    public Bishop(Color color)
    {
        Color = color;
        PieceName = "Bishop";
    }
    public override Piece Copy()
    {
        Bishop copy = new Bishop(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }

    public override IEnumerable<Move> GetMoves(Position from, ChessBoard board)
    {
        return MovePositionInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
    }
}

