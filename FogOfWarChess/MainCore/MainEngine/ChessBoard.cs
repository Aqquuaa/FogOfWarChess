using FogOfWarChess.MainCore.MainEngine.ChessPieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FogOfWarChess.MainCore.MainEngine;

public class ChessBoard
{
    public int boardSize;//I've changed protection, so we could change dynamicaly size of a window. alternatively we can open it in standard 1920*1080 
    private ChessTile[,] tiles;

    public ChessBoard(int boardSize = 8)
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
    /// <summary>
    /// Method to set all chess pieces on your size board
    /// </summary>

    public Piece this[int row, int col]
    {
        get { return tiles[row, col].GetPiece(); }
        set { tiles[row, col].SetPiece(value); }
    }
    public Piece this[Position pos]
    {
        get { return tiles[pos.Row, pos.Column].GetPiece(); }
        set { tiles[pos.Row, pos.Column].SetPiece(value);}
    }

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

        for (int i = 0; i < 8; i++)
        {
            tiles[1, i].SetPiece(new Pawn(Color.Black));
            tiles[6, i].SetPiece(new Pawn(Color.White));
        }
    }

    public static ChessBoard SetChessPiecesToTheirPositions()
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

    public void MovePiece(ChessTile startTile, ChessTile finishTile)
    {
        var chessPiece = startTile.GetPiece();
        finishTile.SetPiece(chessPiece);
        startTile.SetPiece(null);
    }

    private bool IsInBounds(int row, int column)
    {
        return row >= 0 && row < boardSize && column >= 0 && column < boardSize;
    }

    // I added this 2 methods so i wouldn't need to change almost all file, but basically it does the same as 2 methods above
    public bool IsInside(Position pos)
    {
        return pos.Row >= 0 && pos.Row < boardSize && pos.Column >= 0 && pos.Column < boardSize;
    }

    public bool IsEmpty(Position pos)
    {
        return this[pos] == null;
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
}