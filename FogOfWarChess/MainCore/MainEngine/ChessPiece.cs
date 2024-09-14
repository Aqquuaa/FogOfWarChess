using System;

namespace FogOfWarChess.MainCore.MainEngine
{

    public enum Color
    {
        None, 
        White,
        Black
    }   
    public static class PlayerExtensions
    {
        public static Color Oponent(this Color player)
        {
            return player switch
            {
                Color.White => Color.Black,
                Color.Black => Color.White,
                _ => Color.None,
            };
        }
    }


    public abstract class ChessPiece : IChessPiece
    {
        public abstract Color Color { get; set; }

        public abstract void Move();
    }
}
