using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FogOfWarChess.MainCore.MainEngine
{
    public abstract class Piece
    {
        public string PieceName { get; protected set; }
        private Texture2D pieceTexture2D;
        public abstract PieceType Type { get; }
        public abstract Color Color { get; }
        public bool HasMoved { get; set; } = false;
        public abstract Piece Copy();

        public void LoadTexture(ContentManager content)
        {
            pieceTexture2D = content.Load<Texture2D>(PieceName + Color);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 positionVector2D)
        {
            spriteBatch.Draw(pieceTexture2D, positionVector2D, Microsoft.Xna.Framework.Color.White);
        }
    }

}