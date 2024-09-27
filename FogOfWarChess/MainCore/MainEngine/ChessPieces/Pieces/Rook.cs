namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class Rook : Piece
{
    public override PieceType Type => PieceType.Rook;
    public override Color Color { get; }

    public Rook(Color color)
    {
        Color = color;
        PieceName = "Rook";
    }
    public override Piece Copy()
    {
        Rook copy = new Rook(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }
}
