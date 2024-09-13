namespace FogOfWarChess.MainCore.MainEngine;

public class ChessBoard
{
    private int boardSize;
    private ChessTile[,] tiles;

    public ChessBoard(int boardSize = 8)
    {
        this.boardSize = boardSize;

        for (int row = 0; row < boardSize; row++)
        {
            for (int column = 0; column < boardSize; column++)
            {
                tiles[row, column] = new ChessTile(row, column);
            }
        }
    }

    public void SetChessPieces()
    {
        //TODO: implement functionality
    }

}