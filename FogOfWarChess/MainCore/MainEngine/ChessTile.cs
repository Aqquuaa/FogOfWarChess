namespace FogOfWarChess.MainCore.MainEngine;

public class ChessTile
{
    private int row;
    private int column;
    private IChessPiece chessPiece;

    public ChessTile(int row, int column, IChessPiece chessPiece = null)
    {
        this.row = row;
        this.column = column;
    }
    public void SetPiece(IChessPiece _chessPiece)
    {
        chessPiece = _chessPiece;
    }

    public IChessPiece GetPiece()
    {
        return chessPiece;
    }
}