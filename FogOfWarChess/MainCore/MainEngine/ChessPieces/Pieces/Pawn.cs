using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;

namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class Pawn : Piece
{
    public override PieceType Type => PieceType.Pawn;
    public override Color Color { get; }
    private readonly Direction forward;

    public Pawn(Color color)
    {
        Color = color;
        if(color == Color.White)
        {
            forward = Direction.North;
        }
        else if (color == Color.Black)
        {
            forward = Direction.South;
        }

        PieceName = "Pawn";
    }
    public override Piece Copy()
    {
        Pawn copy = new Pawn(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }

    private static bool CanMoveTo(Position pos, ChessBoard board)
    {
        return board.IsInside(pos) && board.IsEmpty(pos);
    }

    private bool CanCapture(Position pos, ChessBoard board)
    {
        if (!board.IsInside(pos) || board.IsEmpty(pos))
        {
            return false;
        }
        return board[pos].Color != Color;
    }

    private IEnumerable<Move> ForwardMoves(Position from, ChessBoard board)
    {
        Position MoveOneTile = from + forward;

        if(CanMoveTo(MoveOneTile, board))
        {
            yield return new NormalMove(from, MoveOneTile);

            Position MoveTwoTiles = MoveOneTile + forward;

            if(!HasMoved && CanMoveTo(MoveTwoTiles, board))
            {
                yield return new NormalMove(from, MoveTwoTiles);
            }
        }
    }

    private IEnumerable<Move> DiagonalMoves(Position from, ChessBoard board)
    {
        foreach (Direction dir in new Direction[] { Direction.West, Direction.East} )
        {
            Position to = from + forward + dir;

            if(CanCapture(to, board))
            {
                yield return new NormalMove(from, to);
            }
        }
    }

    public override IEnumerable<Move> GetMoves(Position from, ChessBoard board)
    {
        return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
    }

    public override bool EnemiesKingCanBeCaptured(Position from, ChessBoard board)
    {
        return DiagonalMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King;
            });
    }
}
