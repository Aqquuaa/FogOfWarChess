namespace FogOfWarChess.MainCore.MainEngine;

public class ChessBoard
{
    private int boardSize;
    private ChessTile[,] tiles;

    public ChessBoard(int boardSize = 8)
    {
        this.boardSize = boardSize;

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
    public void SetChessPieces()
    {
        //TODO: implement functionality
        //Need chess piece classes to do that 
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
}