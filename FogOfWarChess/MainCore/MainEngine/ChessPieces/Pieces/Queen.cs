using System.Collections.Generic;
using System.Linq;

namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class Queen : Piece
{
    public override PieceType Type => PieceType.Queen;
    public override Color Color { get; }
    private static readonly Direction[] dirs = new Direction[]
    {
        Direction.North,
        Direction.South,
        Direction.East,
        Direction.West,
        Direction.NorthWest,
        Direction.NorthEast,
        Direction.SouthWest,
        Direction.SouthEast,
    };
    
    public Queen(Color color)
    {
        Color = color;
        PieceName = "Queen";
    }
    public override Piece Copy()
    {
        Queen copy = new Queen(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }

    public override IEnumerable<Move> GetMoves(Position from, ChessBoard board)
    {
        return MovePositionInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
    }
}
