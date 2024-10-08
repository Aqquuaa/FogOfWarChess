using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace FogOfWarChess.MainCore.MainEngine;

class Camera 
{
    private Matrix transform;
    public Matrix Transform {get { return transform; }}

    private Vector2 centre;
    private Viewport viewport;
    private float zoom = 1;
    private float rotation = 0;

    public float X
    {
        get { return centre.X; }
        set { centre.X = value; }
    }
    
    public float Y
    {
        get { return centre.Y; }
        set { centre.Y = value; }
    }

    public float Zoom
    {
        get { return zoom; } 
        set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
    }

    public float Rotation 
    {
        get { return rotation; }
        set { rotation = value; }
    }

    public Camera(Viewport newViewport)
    {
        viewport = newViewport;
    }

    public void Update(Vector2 position)
    {
        centre = new Vector2(position.X,position.Y);
        transform = Matrix.CreateTranslation(new Vector3 (-centre.X, -centre.Y, 0)) *
                                                          Matrix.CreateRotationZ(Rotation) *
                                                          Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
                                                          Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
    }
}