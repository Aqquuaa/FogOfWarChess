namespace FogOfWarChess.MainCore.MainEngine;

public class ChessTile
{
    private int row;
    private int column;
    private ChessPiece chessPiece;

    public ChessTile(int row, int column, ChessPiece chessPiece = null)
    {
        this.row = row;
        this.column = column;
    }
}