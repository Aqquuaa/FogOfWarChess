using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace FogOfWarChess.MainCore.MainEngine;

public class User
{
    private ButtonState previousLeftButtonState = ButtonState.Released;
    private HandlingMoves handlingMoves;
    public NormalMove SelectedMove { get; set; }
    public bool HasMove { get; private set; }
    private Color userColor = Color.White;
    private IEnumerable<Position> movePositions = null;

    //We get Input From user
    public void GetUserInputForGame(KeyboardState keyboardState, MouseState mouseState, ChessBoard chessBoard)
    {
        SelectPiece(mouseState, chessBoard);
        MediaPlayerVolumeChange(keyboardState);
        DebugColorChange(keyboardState);
    }

    public void InitUser(ChessBoard chessBoard, string color)
    {   
        userColor = color == "White" ? Color.White : Color.Black;
        Debug.WriteLine(color);
        handlingMoves = new HandlingMoves(userColor, chessBoard);
        chessBoard.SetFog(userColor);
        DebugFogSet(chessBoard);
    }

    private void DebugColorChange(KeyboardState keyboardState)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
            userColor = Color.White;
        if (Keyboard.GetState().IsKeyDown(Keys.B))
            userColor = Color.Black;
    }

    //If we want, we can change how load music plays (I think we'll need it relocate)
    static void MediaPlayerVolumeChange(KeyboardState keyboardState)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            MediaPlayer.Volume += +0.02f;
        if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            MediaPlayer.Volume += -0.02f;
    }

    //We get mouse coordinates as vector 
    private static Vector2 GetMouseCoordinates(MouseState mouseState)
    {
        Point position = mouseState.Position;
        return new Vector2(position.X, position.Y);
    }
    public void CallFog(ChessBoard chessBoard)
    {
        chessBoard.SetFog(userColor);
        DebugFogSet(chessBoard);
    }

    private void DebugFogSet(ChessBoard chessBoard)//i'll change this method to the fast one soon
    {
        for (int i = 0; i < GlobalVariables.sizeOfBoard; i++)
        {
            for (int j = 0; j < GlobalVariables.sizeOfBoard; j++)
            {
                Position clickedPosition = new Position(i, j);
                Piece selectedPiece = chessBoard[i, j];
                if (selectedPiece != null && selectedPiece.Color == userColor)
                {
                    IEnumerable<Move> moves = handlingMoves.LegalMovesForPiece(clickedPosition, chessBoard);

                    if (moves.Any())
                    {
                        CacheMoves(moves);
                        movePositions = null;
                        movePositions = moves.Select(move => move.ToPos);
                        chessBoard.ClearFog(userColor, movePositions);
                        handlingMoves.moveCache.Clear();
                    }
                }
            }
        }
    }
    /// <summary>
    /// Method to select piece we want to move
    /// We need to refactor this
    /// </summary>
    private void SelectPiece(MouseState mouseState, ChessBoard chessBoard)
    {
        int column, row;
        Vector2 mouseCoordinates;
        //Function works only after user releases left button
        if (previousLeftButtonState == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
        {
            //Row - y, Column - x
            mouseCoordinates = GetMouseCoordinates(mouseState);
            row = (int)mouseCoordinates.Y / GlobalVariables.tileSize;
            column = (int)mouseCoordinates.X / GlobalVariables.tileSize;

            if (userColor == Color.Black)
            {
                row = GlobalVariables.sizeOfBoard - 1 - row;
                column = GlobalVariables.sizeOfBoard - 1 - column;
            }

            Console.WriteLine("MouseCoordinates X.{0} Y.{1}", mouseCoordinates.X, mouseCoordinates.Y);
            Console.WriteLine("ClickedPosition {0} {1}", column, row);

            //Check if mouse is inside of game window. Probably inside of monoGame there is easier method
            if (IsInsideOfGame(mouseCoordinates) && column <= GlobalVariables.sizeOfBoard && row <= GlobalVariables.sizeOfBoard)
            {
                Position clickedPosition = new Position(row, column);
                Piece selectedPiece = chessBoard[clickedPosition];
                if (movePositions != null)
                {
                    chessBoard.ForgetPossibleMoves(movePositions);
                }

                if (selectedPiece != null && selectedPiece.Color == userColor)
                {
                    FromPositionSel(clickedPosition, chessBoard);
                }
                else
                {
                    chessBoard.SetFog(userColor);//test method to set fog for the current player
                    ToPositionSel(clickedPosition, chessBoard);
                    DebugFogSet(chessBoard);//test method to remove fog for the current player
                }
            }
        }
        previousLeftButtonState = mouseState.LeftButton;
    }

    private void FromPositionSel(Position pos, ChessBoard chessBoard) // We call this method, if we have no selected piece
    {
        //We create a new list of moves, that our piece can make. 
        IEnumerable<Move> moves = handlingMoves.LegalMovesForPiece(pos, chessBoard);
        Console.WriteLine("From");
        //If we have any moves in our list, we cache this moves and execute the move.
        if (moves.Any())
        {
            CacheMoves(moves);
            movePositions = null;
            movePositions = moves.Select(move => move.ToPos);
            chessBoard.DrawPossibleMoves(movePositions);
        }
    }

    private void ToPositionSel(Position pos, ChessBoard chessBoard)
    {
        Console.WriteLine("To");
        //selectedPos = null;
        if (handlingMoves.moveCache.TryGetValue(pos, out Move move))
        {
            Console.WriteLine(move.GetType());
            SelectedMove = (NormalMove)move;
            HasMove = true;
            HandleMove(move, chessBoard);
        }
        handlingMoves.moveCache.Clear();
    }

    private void HandleMove(Move move, ChessBoard chessBoard)
    {
        handlingMoves.MakeMove(move, chessBoard);
    }

    private void CacheMoves(IEnumerable<Move> moves)
    {
        handlingMoves.moveCache.Clear();
        foreach (Move move in moves)
        {
            handlingMoves.moveCache[move.ToPos] = move;
        }
    }

    private static bool IsInsideOfGame(Vector2 mouseCoord)
    {
        if (mouseCoord.X > 0 && mouseCoord.X < GlobalVariables.sizeOfBoard * GlobalVariables.tileSize &&
            mouseCoord.Y > 0 && mouseCoord.Y < GlobalVariables.sizeOfBoard * GlobalVariables.tileSize)
            return true;
        return false;
    }

    public Color Color
    {
        get { return userColor; }
        set { userColor = value; }
    }
}