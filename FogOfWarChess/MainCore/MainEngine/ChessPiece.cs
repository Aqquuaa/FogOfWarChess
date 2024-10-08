﻿namespace FogOfWarChess.MainCore.MainEngine

{
    public enum Color
    {
        None, 
        White,
        Black
    }   
    public static class PlayerExtensions
    {
        public static Color Opponent(this Color player)
        {
            return player switch
            {
                Color.White => Color.Black,
                Color.Black => Color.White,
                _ => Color.None,
            };
        }
    }

}
