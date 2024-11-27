using System;
using System.Collections.Generic;
using System.Linq;

namespace FogOfWarChess.MainCore.MainEngine;

public class HandlingMoves
{
    public ChessBoard Board { get; }
    public Color CurrentPlayersColor { get; private set; }
    public Result Result { get; private set; } = null; 
    public readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();

    public HandlingMoves(Color color, ChessBoard board)
    {
        CurrentPlayersColor = color;
        Board = board;
    }

    public IEnumerable<Move> LegalMovesForPiece(Position pos, ChessBoard board)
    {
        if (board.IsEmpty(pos)|| board[pos].Color != CurrentPlayersColor)
        {
            Console.WriteLine("Not Legal");
            return Enumerable.Empty<Move>();
        }

        Console.WriteLine("Legal");
        Piece piece = board[pos];
        IEnumerable<Move> piecesCanBeMoved = piece.GetMoves(pos, board);
        return piecesCanBeMoved;//.Where(move => move.IsMoveSaveForKing(board)); //I had to comment it because game doesn't work properly with thie method
    }

    public void MakeMove(Move move, ChessBoard board)
    {
        
        move.Execute(board);
        //CurrentPlayersColor = CurrentPlayersColor.Opponent();
        CheckForGameover();
    }

    public IEnumerable<Move> AllLegalMovesForPlayer(Color color)
    {
        IEnumerable<Move> moveCandidates = Board.GetAllPiecePositionsOfColor(color).SelectMany(pos =>
        {
            Piece piece = Board[pos];
            return piece.GetMoves(pos, Board);
        });

        return moveCandidates.Where(move => move.IsMoveSaveForKing(Board));
    }

    private void CheckForGameover()
    {
        if(!AllLegalMovesForPlayer(CurrentPlayersColor).Any())
        {
            if(Board.IsInCheck(CurrentPlayersColor))
            {
                Result = Result.Win(CurrentPlayersColor.Opponent());
            }
            else
            {
                Result = Result.Draw(EndReason.Checkmate);
            }
        }

        Console.WriteLine("Our result of the game is {0}", Result);
    }
}