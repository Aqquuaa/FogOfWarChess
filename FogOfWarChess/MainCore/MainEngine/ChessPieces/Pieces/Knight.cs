namespace FogOfWarChess.MainCore.MainEngine.ChessPieces;

public class Knight : Piece
{
    public override PieceType Type => PieceType.Knight;
    public override Color Color { get; }

    public Knight(Color color)
    {
        Color = color;
        PieceName = "Knight";
    }
    public override Piece Copy()
    {
        Knight copy = new Knight(Color);
        copy.HasMoved = HasMoved;
        return copy;    
    }
}
