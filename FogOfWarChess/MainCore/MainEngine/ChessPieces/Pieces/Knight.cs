using System.Collections.Generic;
using System.Linq;

namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class Knight : Piece
{
    public override PieceType Type => PieceType.Knight;
    public override Color Color { get; }
    
    public Knight(Color color)
    {
        Color = color;
        PieceName = "Knight";
    }
    public override Piece Copy()
    {
        Knight copy = new Knight(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }

    // so we just get our position, move 1 tile vertically(vDir) and 1*2 tiles horizontally
    private static IEnumerable<Position> AllPossiblePositions(Position from) 
    {
        foreach (Direction vDir in new Direction[] { Direction.North, Direction.South})
        {
            foreach (Direction hDir in new Direction[] { Direction.West, Direction.East})
            {
                yield return from + hDir + (2 * vDir); // for some strange reason, we can't write vDir * 2, but can 2 * vDir
                yield return from + vDir + (2 * hDir);
            }
        }
    }

    private IEnumerable<Position> MovePositions(Position from, ChessBoard board)
    {
        return AllPossiblePositions(from).Where(pos => board.IsInside(pos) && (board.IsEmpty(pos) || board[pos].Color != Color));
    }

    public override IEnumerable<Move> GetMoves(Position from, ChessBoard board)
    {
        return MovePositions(from, board).Select(to => new NormalMove(from, to));
    }
}
