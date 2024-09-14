using System;

namespace FogOfWarChess.MainCore.MainEngine;

public interface IChessPiece
{
    Color Color { get; set; }

    //ChessPiece texture

    //ChessPiece location

    void Move();
}
