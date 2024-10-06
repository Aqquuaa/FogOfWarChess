using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;

namespace FogOfWarChess.MainCore.MainEngine;

public class User
{
    //public Color color;
    private ButtonState previousLeftButtonState = ButtonState.Released;
    public void GetUserInput(KeyboardState keyboardState, MouseState mouseState, ChessBoard chessBoard)
    {
        SelectPiece(mouseState, chessBoard);
        MediaPlayerVolumeChange(keyboardState);

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
            Vector2 mouseCoordinates = GetMouseCoordinates(mouseState);
            column = (int)mouseCoordinates.X / tileSize;
            row = (int)mouseCoordinates.Y / tileSize;
            //Debug.Print("ClickedPosition {0} {1}", column, row);
            if (column <= GlobalVariables.sizeOfBoard && row <= GlobalVariables.sizeOfBoard)
            {
                Position clickedPosition = new Position(column, row);

                Piece selectedPiece = chessBoard[clickedPosition];

                if (selectedPiece != null)
                {
                    IEnumerable<Move> possibleMoves = selectedPiece.GetMoves(clickedPosition, chessBoard);

                    IEnumerable<Position> movePositions = possibleMoves.Select(move => move.ToPos);

                    chessBoard.DrawPossibleMoves(movePositions);
                }
            }
        }
        previousLeftButtonState = mouseState.LeftButton;
    }


}