using System.Collections.Generic;
using System.Linq;

namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class Rook : Piece
{
    public override PieceType Type => PieceType.Rook;
    public override Color Color { get; }
    private static readonly Direction[] dirs = new Direction[]
    {
        Direction.North,
        Direction.South,
        Direction.East,
        Direction.West,
     };

    public Rook(Color color)
    {
        Color = color;
        PieceName = "Rook";
    }
    public override Piece Copy()
    {
        Rook copy = new Rook(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }

    public override IEnumerable<Move> GetMoves(Position from, ChessBoard board)
    {
        return MovePositionInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
    }
}
