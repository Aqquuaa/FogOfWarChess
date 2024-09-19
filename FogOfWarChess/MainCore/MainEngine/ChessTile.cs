using FogOfWarChess.MainCore.MainEngine.ChessPieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FogOfWarChess.MainCore.MainEngine;

public class ChessTile
{
    private int row;
    private int column;
    private Piece chessPiece;

    private Texture2D darkTileTexture2D;
    private Texture2D lightTileTexture2D;
    //private Texture2D pawnBlack2D;
    public ChessTile(int row, int column, Piece chessPiece = null)
    {
        this.row = row;
        this.column = column;
    }
    public void SetPiece(Piece _chessPiece)
    {
        chessPiece = _chessPiece;
    }

    public Piece GetPiece()
    {
        return chessPiece;
    }

    public void LoadContent(ContentManager content)
    {
        darkTileTexture2D = content.Load<Texture2D>("DarkTile");
        lightTileTexture2D = content.Load<Texture2D>("LightTile");
        //pawnBlack2D = content.Load<Texture2D>("PawnBlack");
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        Texture2D tileTexture = (row + column) % 2 == 0 ? lightTileTexture2D : darkTileTexture2D;
        spriteBatch.Draw(tileTexture, position, Microsoft.Xna.Framework.Color.White);

        if (chessPiece != null)
        {
            //chessPiece.Draw(spriteBatch, position);
        }
    }
}