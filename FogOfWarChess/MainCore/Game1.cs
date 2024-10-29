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
    private User user;
    private Song song;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        chessBoard = new ChessBoard();
        user = new User();
        user.InitHandlingMoves();
        WindowSizeFind(chessBoard.boardSize);

        base.Initialize();
    }

    protected void WindowSizeFind(int sizeOfBoard)
    {
        int Width = sizeOfBoard * 40;
        int Height = sizeOfBoard * 40;
        WindowSizeApply(Width);
    }

    protected void WindowSizeApply(int Width)
    {
        _graphics.PreferredBackBufferWidth = Width;
        _graphics.PreferredBackBufferHeight = Width;
        _graphics.ApplyChanges();

        //Or we can set it fullscreen. In this case, return boardsize protection back to private. Also if will use it, we will have standard blue background
        //But also we will have a bug with camera rotation
        /*_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.ApplyChanges();*/

    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        this.song = Content.Load<Song>("Sounds/Music/BackgroundMusic");
        MediaPlayer.Play(song);
        MediaPlayer.Volume = 0.5f;
        MediaPlayer.IsRepeating = true;
        chessBoard.LoadTexture(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        // TODO: Add your update logic here


        HandleInput(gameTime);

        base.Update(gameTime);
    }

    private void HandleInput(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();
        user.GetUserInputForGame(keyboardState, mouseState, chessBoard);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
        if (user.Color == MainCore.MainEngine.Color.White)
        {
            chessBoard.Draw(_spriteBatch);
        }
        else chessBoard.DrawInverted(_spriteBatch);

        _spriteBatch.End();
        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}