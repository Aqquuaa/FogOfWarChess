using System.Collections;
using System.Collections.Generic;

namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class King : Piece
{
    public override PieceType Type => PieceType.King;
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

    public King(Color color)
    {
        Color = color;
        PieceName = "King";
    }
    public override Piece Copy()
    {
        King copy = new King(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }

    private IEnumerable<Position> MovePositions (Position from, ChessBoard board)
    {
        foreach (Direction dir in dirs)
        {
            Position to = from + dir;

            if (!board.IsInside(to))
            {
                continue;
            }

            if (board.IsEmpty(to) || board[to].Color != Color)
            {
                yield return to;
            }
        }
    }

    public override IEnumerable<Move> GetMoves(Position from, ChessBoard board)
    {
        foreach (Position to in MovePositions(from, board))
        {
            yield return new NormalMove(from, to);
        }
    }
}
