using System;
using System.Collections.Generic;
using System.Linq;

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

    private static bool RookUnmoved(Position pos, ChessBoard board)
    {
        if(board.IsEmpty(pos))
        {
            return false;
        }

        Piece piece = board[pos];
        return piece.Type == PieceType.Rook && !piece.HasMoved;
    }

    private static bool PositionBetweenEmptyCastling(IEnumerable<Position> positions, ChessBoard board)
    {
        return positions.All(pos => board.IsEmpty(pos));
    }   

    private bool CanCastleKingSite(Position from, ChessBoard board)
    {
        if(HasMoved)
        {
            return false;
        }

        Position rookPos = new Position(from.Row, 7);
        Position[] positionsBetween = new Position[] {new(from.Row, 5), new(from.Row, 6)};

        return RookUnmoved(rookPos, board) && PositionBetweenEmptyCastling(positionsBetween, board);
    }

        private bool CanCastleQueenSite(Position from, ChessBoard board)
    {
        if(HasMoved)
        {
            return false;
        }

        Position rookPos = new Position(from.Row, 0);
        Position[] positionsBetween = new Position[] {new(from.Row, 1), new(from.Row, 2), new(from.Row, 3)};

        return RookUnmoved(rookPos, board) && PositionBetweenEmptyCastling(positionsBetween, board);
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

        if (CanCastleKingSite(from, board))
        {
            yield return new Castling(MoveType.CastlingKingSite, from);
        }

        if (CanCastleQueenSite(from, board))
        {
            yield return new Castling(MoveType.CastlingQueenSite, from);
        }
    }

    public override bool EnemiesKingCanBeCaptured(Position from, ChessBoard board)
    {
        return MovePositions(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Type == PieceType.King;
            });
    }   
}

public class Castling : Move
{
    public override MoveType Type { get; } 
    public override Position FromPos { get; }
    public override Position ToPos { get; }

    private readonly Direction kingMoveDir;
    private readonly Position rookFromPos;
    private readonly Position rookToPos;

    public Castling(MoveType type, Position kingPos)
    {
        Type = type;
        FromPos = kingPos;

        if(type == MoveType.CastlingQueenSite)
        {
            kingMoveDir = Direction.West;
            ToPos = new Position(kingPos.Row, 2);
            rookFromPos = new Position(kingPos.Row, 0);
            rookToPos = new Position(kingPos.Row, 3);
        }
        else if (type == MoveType.CastlingKingSite)
        {
            kingMoveDir = Direction.East;
            ToPos = new Position(kingPos.Row, 6);
            rookFromPos = new Position(kingPos.Row, 7);
            rookToPos = new Position(kingPos.Row, 5);
        }
    }

    public override void Execute(ChessBoard board)
    {
        new NormalMove(FromPos, ToPos).Execute(board);
        new NormalMove(rookFromPos, rookToPos).Execute(board);
    }

    public override bool IsMoveSaveForKing(ChessBoard board) // Castling should be safe for a king
    {
        Color color = board[FromPos].Color;

        if(board.IsInCheck(color))
        {
            return false;
        }

        ChessBoard copy = board.Copy();
        Position kingCopiesPos = FromPos;

        for(int i = 0; i < 2; i++)
        {
            new NormalMove(kingCopiesPos, kingCopiesPos + kingMoveDir).Execute(copy);
            kingCopiesPos += kingMoveDir;

            if (copy.IsInCheck(color))
            {
                Console.WriteLine("Castling is not safe!");
                return false;
            }
        }

        Console.WriteLine("Castling is safe!");
        return true;
    }
}
