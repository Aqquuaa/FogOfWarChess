namespace FogOfWarChess.MainCore.MainEngine
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Color Color { get; }
        public bool HasMoved { get; set; } = false;
        public abstract Piece Copy();
    }

}