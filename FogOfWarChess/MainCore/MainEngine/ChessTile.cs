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
    private bool fogOfWar;
    private bool possibleMove;

    private Texture2D darkTileTexture2D;
    private Texture2D lightTileTexture2D;
    private Texture2D possibleMoveTexture2D;
    private Texture2D fogTexture2D;

    public ChessTile(int row, int column, Piece chessPiece = null)
    {
        this.row = row;
        this.column = column;
        this.possibleMove = false;
        this.fogOfWar = false;
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

    public void SetPossibleMoveToFalse()
    {
        possibleMove = false;
    }

    public void SetFog()
    {
        fogOfWar = true;
    }

    public void SetFogToFalse()
    {
        fogOfWar = false;
    }

    public void LoadContent(ContentManager content)
    {
        darkTileTexture2D = content.Load<Texture2D>("CoreTextures/DarkTile");
        lightTileTexture2D = content.Load<Texture2D>("CoreTextures/LightTile");
        possibleMoveTexture2D = content.Load<Texture2D>("CoreTextures/RedTile");
        fogTexture2D = content.Load<Texture2D>("CoreTextures/FogTile");
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        if (fogOfWar)
        {
            spriteBatch.Draw(fogTexture2D, position, Microsoft.Xna.Framework.Color.White);
        }
        else
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

}