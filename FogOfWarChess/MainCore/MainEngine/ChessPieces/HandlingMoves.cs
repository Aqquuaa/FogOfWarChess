using System;
using System.Collections.Generic;
using System.Linq;

namespace FogOfWarChess.MainCore.MainEngine;

public class HandlingMoves
{
    public ChessBoard Board { get; }
    public Color CurrentPlayersColor { get; private set; }
    public readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();

    public HandlingMoves(Color color, ChessBoard board)
    {
        CurrentPlayersColor = color;
        Board = board;
    }

    public IEnumerable<Move> LegalMoves(Position pos, ChessBoard board)
    {
        if (board.IsEmpty(pos)|| board[pos].Color != CurrentPlayersColor)
        {
            Console.WriteLine("Not Legal");
            return Enumerable.Empty<Move>();
        }

        Console.WriteLine("Legal");
        Piece piece = board[pos];
        return piece.GetMoves(pos, board);
    }

    public void MakeMove(Move move, ChessBoard board)
    {
            move.Execute(board);
            CurrentPlayersColor = CurrentPlayersColor.Opponent();
    }

    public void CacheMoves(IEnumerable<Move> moves)
    {
        moveCache.Clear();

        foreach(Move move in moves)
        {
            moveCache[move.ToPos] = move;
        }
    }
}