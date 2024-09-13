using System;

namespace FogOfWarChess.MainCore.MainEngine
{

    public enum Color
    {
        White,
        Black
    }


    public abstract class ChessPiece : IChessPiece
    {
        public abstract Color Color { get; set; }

        public abstract void Move();
    }
}
