using FogOfWarChess.MainCore.MainEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Color = Microsoft.Xna.Framework.Color;

namespace FogOfWarChess;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private ChessBoard chessBoard;
    private Song song;
    Vector2 playerPoz;

    Camera2D cam_white_player = new Camera2D(),
             cam_black_player = new Camera2D();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        chessBoard = new ChessBoard();
        WindowSize(chessBoard.boardSize);

        base.Initialize();
    }

    protected void WindowSize(int sizeOfBoard)
    {
        _graphics.PreferredBackBufferWidth =  sizeOfBoard * 40;
        _graphics.PreferredBackBufferHeight = sizeOfBoard * 40;
        _graphics.ApplyChanges();

        //Or we can set it fullscreen. In this case, return boardsize protection back to private. Also if will use it, we will have standard blue background
        /*_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.ApplyChanges();*/
        
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        this.song = Content.Load<Song>("BackgroundMusic");
        MediaPlayer.Play(song);
        MediaPlayer.Volume = 0.5f;
        MediaPlayer.IsRepeating = true;
        chessBoard.LoadTexture(Content);
    }

    void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
    {
        // 0.0f is silent, 1.0f is full volume
        MediaPlayer.Volume -= 0.1f;
        MediaPlayer.Play(song);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        if (Keyboard.GetState().IsKeyDown(Keys.W))
            cam_white_player.Rotation = 0.5f;
        if (Keyboard.GetState().IsKeyDown(Keys.B))
            cam_black_player.Rotation = 0f;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        chessBoard.Draw(_spriteBatch);
        _spriteBatch.End();
        // TODO: Add your drawing code here

        //CAMERA
        //WHITE PLAYER
        /*_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
        cam_white_player.get_transformation(GraphicsDevice));
        //_spriteBatch.Draw(texture, new Vector2(180, 130), Color.Yellow);
        _spriteBatch.End();
        //BLACK PLAYER
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
        cam_black_player.get_transformation(GraphicsDevice ));
        //_spriteBatch.Draw(chessBoard.LoadContent(Content), new Vector2(180, 130), Color.Yellow);
        _spriteBatch.End();*/
        //CAMERA
        base.Draw(gameTime);
    }

    protected void CameraIni()
    {
        playerPoz = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

        cam_white_player.Pos = playerPoz;
        cam_white_player.Zoom = 1;
        cam_white_player.Rotation = 0;

        cam_black_player.Pos = cam_white_player.Pos;
        cam_black_player.Zoom = 1;
        cam_black_player.Rotation = 0.5f;
    }
}
