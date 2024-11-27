using System;
using FogOfWarChess.MainCore.MainEngine.ChessPieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FogOfWarChess.MainCore.MainEngine;

public class ChessBoard
{
    public int boardSize;
    private readonly ChessTile[,] tiles;

    public ChessBoard(int boardSize = GlobalVariables.sizeOfBoard)
    {
        this.boardSize = boardSize;

        tiles = new ChessTile[boardSize, boardSize];

        for (var row = 0; row < boardSize; row++)
        {
            for (var column = 0; column < boardSize; column++)
            {
                tiles[row, column] = new ChessTile(row, column);
            }
        }

        SetChessPiecesPositions();
    }

    public Piece this[int row, int col]
    {
        get { return tiles[row, col].GetPiece(); }
        set { tiles[row, col].SetPiece(value); }
    }

    public Piece this[Position pos]
    {
        get { return tiles[pos.Row, pos.Column].GetPiece(); }
        set { tiles[pos.Row, pos.Column].SetPiece(value); }
    }

    /// <summary>
    /// Method to set all chess pieces on your size board
    /// </summary>
    //If we will make any more pieces, we should use other method, that would be more dynamical
    public void SetChessPiecesPositions()
    {
        tiles[0, 0].SetPiece(new Rook(Color.Black));
        tiles[0, 1].SetPiece(new Knight(Color.Black));
        tiles[0, 2].SetPiece(new Bishop(Color.Black));
        tiles[0, 3].SetPiece(new Queen(Color.Black));
        tiles[0, 4].SetPiece(new King(Color.Black));
        tiles[0, 5].SetPiece(new Bishop(Color.Black));
        tiles[0, 6].SetPiece(new Knight(Color.Black));
        tiles[0, 7].SetPiece(new Rook(Color.Black));

        tiles[7, 0].SetPiece(new Rook(Color.White));
        tiles[7, 1].SetPiece(new Knight(Color.White));
        tiles[7, 2].SetPiece(new Bishop(Color.White));
        tiles[7, 3].SetPiece(new Queen(Color.White));
        tiles[7, 4].SetPiece(new King(Color.White));
        tiles[7, 5].SetPiece(new Bishop(Color.White));
        tiles[7, 6].SetPiece(new Knight(Color.White));
        tiles[7, 7].SetPiece(new Rook(Color.White));

        for (int i = 0; i < boardSize; i++)
        {
            tiles[1, i].SetPiece(new Pawn(Color.Black));
            tiles[6, i].SetPiece(new Pawn(Color.White));
        }
    }

    //This part of class is not used. I guess we can delete it
    /*public static ChessBoard SetChessPiecesToTheirPositions()
    {
        ChessBoard chessBoard = new ChessBoard();
        chessBoard.SetChessPiecesPositions();
        return chessBoard;
    }
    public bool IsTileEmpty(int row, int column)
    {
        return IsInBounds(row, column) && tiles[row, column].GetPiece() == null;
    }

    public bool IsEnemyPiece(int row, int column, Color pieceColor)
    {
        if (!IsInBounds(row, column)) return false;

        var piece = tiles[row, column].GetPiece();
        return piece != null && piece.Color != pieceColor;
    }

    public ChessTile GetTile(int row, int column)
    {
        if (IsInBounds(row, column))
        {
            return tiles[row, column];
        }
        return null;
    }

    /// <summary>
    /// Method to set some piece to some tile
    /// </summary>

    public static void MovePiece(ChessTile startTile, ChessTile finishTile)
    {
        var chessPiece = startTile.GetPiece();
        finishTile.SetPiece(chessPiece);
        startTile.SetPiece(null);
    }

    private bool IsInBounds(int row, int column)
    {
        return row >= 0 && row < boardSize && column >= 0 && column < boardSize;
    }*/

    // I added this 2 methods so i wouldn't need to change almost all file, but basically it does the same as 2 methods above
    public bool IsInside(Position pos)
    {
        return pos.Row >= 0 && pos.Row < boardSize && pos.Column >= 0 && pos.Column < boardSize;
    }

    public bool IsEmpty(Position pos)
    {
        return this[pos] == null;
    }

    //This method return ALL tiles with pieces
    public IEnumerable<Position> GetAllPiecesPositions()
    {
        for (int i = 0; i < GlobalVariables.sizeOfBoard; i++)
        {
            for (int j = 0; j < GlobalVariables.sizeOfBoard; j++)
            {
                Position pos = new Position(i, j);

                if (!IsEmpty(pos))
                    yield return pos;
            }
        }
    }

    //This method return all tiles with pieces of given player(color)
    public IEnumerable<Position> GetAllPiecePositionsOfColor(Color color)
    {
        return GetAllPiecesPositions().Where(pos => this[pos].Color == color);
    }

    //Small check, if King can be captured
    public bool IsInCheck(Color color)
    {
        return GetAllPiecePositionsOfColor(color.Opponent()).Any(pos =>
        {
            Piece piece = this[pos];
            return piece.EnemiesKingCanBeCaptured(pos, this);
        });
    }

    /// <summary>
    /// This method creates a new copy of chessboard.
    /// We need this in order to maintain King logic.
    /// How does it work:
    /// As we make a move, move will firstly be made in another board.
    /// Then if King can't be captured after that move, we make it on our main board
    /// </summary>
    public ChessBoard Copy()
    {
        ChessBoard copy = new ChessBoard();

        foreach (Position pos in GetAllPiecesPositions())
        {
            copy[pos] = this[pos].Copy();
        }

        return copy;
    }

    public void DrawPossibleMoves(IEnumerable<Position> position)
    {
        foreach (var pos in position)
        {
            Console.WriteLine("Possible moves are {0} {1}", pos.Column, pos.Row);
            tiles[pos.Row, pos.Column].SetPossibleMove();
        }
    }

    //debug function to set fog, gonna change soon
    public void SetFog(Color userColor)
    {
        for (var row = 0; row < boardSize; row++)
        {
            for (var column = 0; column < boardSize; column++)
            {
                //Position pos = new Position(row, column); // Not needed

                tiles[row, column].SetFog();
            }
        }
    }

    /*need fix later
    public void ClearFogTiles(IEnumerable<Position> position)
    {
        foreach (var pos in position)
        {
            Console.WriteLine("Fog cleared at {0} {1}", pos.Column, pos.Row);
            tiles[pos.Row, pos.Column].SetFogToFalse();
        }
    }
    */

    public void ClearFog(Color userColor, IEnumerable<Position> visiblePositions)
    {
        for (var row = 0; row < boardSize; row++)
        {
            for (var column = 0; column < boardSize; column++)
            {
                var piece = tiles[row, column].GetPiece();
                if (piece != null && piece.Color == userColor)
                {
                    tiles[row, column].SetFogToFalse();
                }

            }
        }
        foreach (var pos in visiblePositions)
        {
            tiles[pos.Row, pos.Column].SetFogToFalse();
            Debug.WriteLine("Fog cleared at {0} {1}", pos.Column, pos.Row);
        }
    }

    //I guess it's faster version of forgetting red tiles. We get list of only legal moves of piece and delete red tiles only on them 
    public void ForgetPossibleMoves(IEnumerable<Position> position)
    {
        foreach (var pos in position)
        {
            Console.WriteLine("we forgot: {0}", tiles[pos.Row, pos.Column]);
            tiles[pos.Row, pos.Column].SetPossibleMoveToFalse();
        }
    }
    public void ApplyMove(NormalMove move)
    {
        move.Execute(this);
    }
    public void LoadTexture(ContentManager content)
    {
        // Load the textures for each tile
        for (var row = 0; row < boardSize; row++)
        {
            for (var column = 0; column < boardSize; column++)
            {
                tiles[row, column].LoadContent(content);

                if (tiles[row, column].GetPiece() != null)
                {
                    tiles[row, column].GetPiece().LoadTexture(content);
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (var row = 0; row < boardSize; row++)
        {
            for (var column = 0; column < boardSize; column++)
            {
                Vector2 position = new Vector2(column * 40, row * 40);
                tiles[row, column].Draw(spriteBatch, position);
            }
        }
    }

    public void DrawInverted(SpriteBatch spriteBatch)
    {
        for (var row = boardSize - 1; row >= 0; row--)
        {
            for (var column = boardSize - 1; column >= 0; column--)
            {
                Vector2 position = new Vector2((boardSize - 1 - column) * 40, (boardSize - 1 - row) * 40);
                tiles[row, column].Draw(spriteBatch, position);
            }
        }
    }

}