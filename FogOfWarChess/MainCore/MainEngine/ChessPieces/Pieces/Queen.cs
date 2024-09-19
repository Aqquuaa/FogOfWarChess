namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class Queen : Piece
{
    public override PieceType Type => PieceType.Queen;
    public override Color Color { get; }

    public Queen(Color color)
    {
        Color = color;
    }
    public override Piece Copy()
    {
        Queen copy = new Queen(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }
}
