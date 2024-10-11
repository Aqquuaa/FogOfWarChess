using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using System;

namespace FogOfWarChess.MainCore.MainEngine;

public class User
{
    private ButtonState previousLeftButtonState = ButtonState.Released;
    private Position selectedPos = null;
    private HandlingMoves handlingMoves;
    ChessBoard chessBoard;

    public void GetUserInput(KeyboardState keyboardState, MouseState mouseState, ChessBoard chessBoard)
    {
        SelectPiece(mouseState, chessBoard);
        MediaPlayerVolumeChange(keyboardState);
    }

    public void InitHandlingMoves()
    {
        handlingMoves = new HandlingMoves(Color.White, chessBoard);
    }

    void MediaPlayerVolumeChange(KeyboardState keyboardState)
    { 
        if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            MediaPlayer.Volume += +0.02f;
        if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            MediaPlayer.Volume += -0.02f;
    }

    private Vector2 GetMouseCoordinates(MouseState mouseState)
    {
        Point position = mouseState.Position;
        return new Vector2(position.X, position.Y);
    }

    /// <summary>
    /// Method to select piece we want to move
    /// </summary>
    private void SelectPiece(MouseState mouseState, ChessBoard chessBoard)
    {
        int tileSize = 40;
        int column, row;
        //Function works only after user releases left button
        if (previousLeftButtonState == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
        {

            //Row - y, Column - x
            Vector2 mouseCoordinates = GetMouseCoordinates(mouseState);
            row = (int)mouseCoordinates.Y / tileSize;
            column = (int)mouseCoordinates.X / tileSize;
            Console.WriteLine("MouseCoordinates X.{0} Y.{1}", mouseCoordinates.X, mouseCoordinates.Y);
            Console.WriteLine("ClickedPosition {0} {1}", column, row);

            //We hide possible moves (red tiles) after each mouse click
            chessBoard.ForgetPossibleMoves();

            //Was a bug, that game would crash if we click outside of the game
            if (mouseCoordinates.X > 0 && mouseCoordinates.X < GlobalVariables.sizeOfBoard * GlobalVariables.tileSize &&
                mouseCoordinates.Y > 0 && mouseCoordinates.Y < GlobalVariables.sizeOfBoard * GlobalVariables.tileSize)
            {
                if (column <= GlobalVariables.sizeOfBoard && row <= GlobalVariables.sizeOfBoard)
                {
                    Position clickedPosition = new Position(row, column);
                    Piece selectedPiece = chessBoard[clickedPosition];
                    //Console.WriteLine(selectedPiece.Color);


                    if (selectedPiece != null && selectedPiece.Color != handlingMoves.CurrentPlayersColor.Opponent())
                    {
                        Console.WriteLine(selectedPiece);
                        FromPositionSel(clickedPosition, chessBoard);
                    }
                    else
                    {
                        ToPositionSel(clickedPosition, chessBoard);
                    }
                }
            
            }
        }
        previousLeftButtonState = mouseState.LeftButton;
    }
    
    private void FromPositionSel(Position pos, ChessBoard chessBoard) // We call this method, if we have no selected piece
    {
        IEnumerable<Move> moves = handlingMoves.LegalMoves(pos, chessBoard);
        Console.WriteLine("From");
        if (moves.Any())
        {
            selectedPos = pos;
            CacheMoves(moves);
            foreach(var move in moves)
                  Console.WriteLine("Moves are cached {0}",move);
            IEnumerable<Position> movePositions = moves.Select(move => move.ToPos);
            chessBoard.DrawPossibleMoves(movePositions);
        }
    }

    private void ToPositionSel(Position pos, ChessBoard chessBoard)
    {
        Console.WriteLine("To, cached; positions {0}, {1}", pos.Column, pos.Row);
        selectedPos = null;
        if(handlingMoves.moveCache.TryGetValue(pos, out Move move))
            HandleMove(move, chessBoard); 

    }   

    private void HandleMove(Move move, ChessBoard chessBoard)
    {
        handlingMoves.MakeMove(move, chessBoard);
    }

    private void CacheMoves(IEnumerable<Move> moves)
    {
        handlingMoves.moveCache.Clear();
        foreach(Move move in moves)
        {
            handlingMoves.moveCache[move.ToPos] = move;
        }
    }
}