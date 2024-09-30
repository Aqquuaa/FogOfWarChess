using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace FogOfWarChess.MainCore.MainEngine;

public class Camera2D
{
    protected float _zoom;
    public Matrix _transform;
    public Vector2 _pos;
    protected float _rotation;

    public Camera2D()
    {
        _zoom = 1.0f;
        _rotation = 0.0f;
        _pos = Vector2.Zero;
    }

    //Functions to manipulate camera's variables
    public float Zoom
    {
        get { return _zoom; }
        set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
    }

    public float Rotation
    {
        get {return _rotation; }
        set { _rotation = value; }
    }

    public void Move(Vector2 amount)
    {
        _pos += amount;
    }

    public Vector2 Pos
    {
        get{ return  _pos; }
        set{ _pos = value; }
    } 
    // End of functions to manipulate variables

    //Calculates all the transformations
    public Matrix get_transformation(GraphicsDevice graphicsDevice)
{
	_transform =       // Thanks to o KB o for this solution
	  Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
								 Matrix.CreateRotationZ(Rotation) *
								 Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
								 Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
	return _transform;
}
}