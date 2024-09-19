namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class Pawn : Piece
{
    public override PieceType Type => PieceType.Pawn;
    public override Color Color { get; }

    public Pawn(Color color)
    {
        Color = color;
    }
    public override Piece Copy()
    {
        Pawn copy = new Pawn(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }
}
