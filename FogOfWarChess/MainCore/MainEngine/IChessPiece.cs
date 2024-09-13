using System;

namespace FogOfWarChess.MainCore.MainEngine;

public interface IChessPiece
{
    Color Color { get; set; }

    //ChessPiece texture

    //ChessPiece location

    void Move();
}

public class Pawn : IChessPiece
{
    public Color Color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Move()
    {
        throw new NotImplementedException();
    }
}