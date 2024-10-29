using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FogOfWarChess.GUI;

abstract class OnScreenObject(Vector2 startPosition, int width, int height, string buttonText, Texture2D objectTexture2D)
{
    private Vector2 startPosition = startPosition;
    private int width = width;
    private int height = height;
    private string buttonText = buttonText;
    private Texture2D objectTexture2D = objectTexture2D;

    public void CollisionCheck(MouseState mouseState)
    {
        if (mouseState.LeftButton == ButtonState.Pressed)
        {

        }
    }
    public void LoadTexture(ContentManager content)
    {
        objectTexture2D = content.Load<Texture2D>("TESTSPLACEHOLDER");
    }

    public void Draw(ContentManager content)
    {

    }
}