using FogOfWarChess.MainCore.MainEngine.ChessPieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FogOfWarChess.MainCore.MainEngine;

public class ChessBoard
{
    private readonly Piece[,] pieces = new Piece[8,8]; // creating rectangular array to store pieces
    private int boardSize;
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
    }
    /// <summary>
    /// Method to set all chess pieces on your size board
    /// </summary>
    public Piece this[int row, int col]
    {
        get { return pieces[row, col]; }
        set { pieces[row, col] = value; }
    }
    public Piece this[Position pos]
    {
        get { return this[pos.Row, pos.Column]; }
        set { this[pos.Row, pos.Column] = value;}
    }

    public void SetChessPiecesPositions()
    {
        this[0, 0] = new Rook(Color.Black);
        this[0, 1] = new Knight(Color.Black);
        this[0, 2] = new Bishop(Color.Black);
        this[0, 3] = new Queen(Color.Black);  
        this[0, 4] = new King(Color.Black);
        this[0, 5] = new Bishop(Color.Black);  
        this[0, 6] = new Rook(Color.Black);
        this[0, 7] = new Knight(Color.Black);

        this[7, 0] = new Rook(Color.White);
        this[7, 1] = new Knight(Color.White);
        this[7, 2] = new Bishop(Color.White);
        this[7, 3] = new Queen(Color.White);  
        this[7, 4] = new King(Color.White);
        this[7, 5] = new Bishop(Color.White);  
        this[7, 6] = new Rook(Color.White);
        this[7, 7] = new Knight(Color.White);

        for (int i = 0; i <= 7; i++)
        {
            this[1, i] = new Pawn(Color.Black);
            this[6, i] = new Pawn(Color.White);
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

    public void LoadTexture(ContentManager content)
    {
        // Load the textures for each tile
        for (var row = 0; row < boardSize; row++)
        {
            for (var column = 0; column < boardSize; column++)
            {
                tiles[row, column].LoadContent(content);
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