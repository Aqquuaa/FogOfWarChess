namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class King : Piece
{
    public override PieceType Type => PieceType.King;
    public override Color Color { get; }

    public King(Color color)
    {
        Color = color;
        PieceName = "King";
    }
    public override Piece Copy()
    {
        King copy = new King(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }
}
