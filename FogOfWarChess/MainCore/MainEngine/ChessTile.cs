using FogOfWarChess.MainCore.MainEngine.ChessPieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FogOfWarChess.MainCore.MainEngine;

public class ChessTile
{
    private int row;
    private int column;
    private Piece chessPiece;
    private bool possibleMove;

    private Texture2D darkTileTexture2D;
    private Texture2D lightTileTexture2D;
    private Texture2D possibleMoveTexture2D;
    public ChessTile(int row, int column, Piece chessPiece = null)
    {
        this.row = row;
        this.column = column;
        this.possibleMove = false;
    }
    public void SetPiece(Piece _chessPiece)
    {
        chessPiece = _chessPiece;
    }

    public Piece GetPiece()
    {
        return chessPiece;
    }

    public void SetPossibleMove()
    {
        possibleMove = true;
    }
    public void LoadContent(ContentManager content)
    {
        darkTileTexture2D = content.Load<Texture2D>("DarkTile");
        lightTileTexture2D = content.Load<Texture2D>("LightTile");
        possibleMoveTexture2D = content.Load<Texture2D>("RedTile");
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        if (possibleMove == false)
        {
            Texture2D tileTexture = (row + column) % 2 == 0 ? lightTileTexture2D : darkTileTexture2D;
            spriteBatch.Draw(tileTexture, position, Microsoft.Xna.Framework.Color.White);
        }
        else
        {
            spriteBatch.Draw(possibleMoveTexture2D, position, Microsoft.Xna.Framework.Color.White);
        }

        if (chessPiece != null)
        {
            chessPiece.Draw(spriteBatch, position);
        }
    }
}