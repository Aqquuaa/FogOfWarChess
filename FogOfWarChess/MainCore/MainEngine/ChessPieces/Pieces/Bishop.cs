namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class Bishop : Piece
{
    public override PieceType Type => PieceType.Bishop;
    public override Color Color { get; }

    public Bishop(Color color)
    {
        Color = color;
        PieceName = "Bishop";
    }
    public override Piece Copy()
    {
        Bishop copy = new Bishop(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }
}
