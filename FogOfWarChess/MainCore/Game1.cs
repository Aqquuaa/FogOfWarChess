using FogOfWarChess.GUI;
using FogOfWarChess.MainCore.MainEngine;
using FogOfWarChess.NetEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace FogOfWarChess;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private ChessBoard chessBoard;
    private User user;
    private LoginScreen loginScreen;
    private CurrentScene currentSceneManager;
    private ClientConnection clientConnection;
    private bool isMyTurn = false;
    private bool isConnecting = true;
    private bool connectionSuccessful = false;
    private Task connectionTask;

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

        WindowSizeFind(chessBoard.boardSize);
        //currentSceneManager = new CurrentScene(CurrentSceneEnum.LoginScreen);
        loginScreen = new LoginScreen();

        clientConnection = new ClientConnection(OnMoveReceived);

        // Start connection attempt in a background task
        connectionTask = Task.Run(() =>
        {
            try
            {
                isMyTurn = clientConnection.InitConnect("127.0.0.1", 11111);
                string userColor = ClientConnection.UserColor;
                connectionSuccessful = true;
                user.InitUser(chessBoard, userColor);
                Debug.WriteLine("Connected");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Connection failed: {e.Message}");
                connectionSuccessful = false;
            }
            finally
            {
                isConnecting = false;
            }
        });

        base.Initialize();
    }


    protected void WindowSizeFind(int sizeOfBoard)
    {
        int Width = sizeOfBoard * 40;
        int Height = sizeOfBoard * 40;
        WindowSizeApply(Width, Height);
    }

    protected void WindowSizeApply(int Width, int Height)
    {
        _graphics.PreferredBackBufferWidth = Width;
        _graphics.PreferredBackBufferHeight = Height;
        _graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        LoadMusic();
        chessBoard.LoadTexture(Content);
        //loginScreen.LoadContent(Content);
    }

    private void LoadMusic()
    {
        Song song;

        song = Content.Load<Song>("Sounds/Music/BackgroundMusic");
        MediaPlayer.Play(song);
        MediaPlayer.Volume = 0.5f;
        MediaPlayer.IsRepeating = true;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            clientConnection.EndConnections();            
            Exit();
        }

        // TODO: Add your update logic here
        /*currentSceneManager.SceneUpdate(loginScreen, gameTime);
        if (currentSceneManager.CurrentScene == CurrentSceneEnum.LoginScreen)
        {
            currentSceneManager.SetScene(CurrentSceneEnum.MainMenuScreen);
        }*/
        HandleInput(gameTime);
        //loginScreen.Update(gameTime);
        base.Update(gameTime);
    }

    private void HandleInput(GameTime gameTime)
    {
        if (!isMyTurn) return;
        KeyboardState keyboardState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();
        user.GetUserInputForGame(keyboardState, mouseState, chessBoard);
        if (user.HasMove && user.SelectedMove != null) 
        {
            NormalMove move = user.SelectedMove; // Retrieve the selected move
            clientConnection.SendMove(move);    // Send the move to the opponent
            chessBoard.ApplyMove(move);        // Apply the move locally
            isMyTurn = false;                  // Wait for opponent's move
            user.SelectedMove = null;
        }
    }
    private void OnMoveReceived(NormalMove move)
    {
        chessBoard.ApplyMove(move);
        user.CallFog(chessBoard);
        isMyTurn = true; // It's now the local player's turn
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
        //currentSceneManager.SceneDraw(_spriteBatch, loginScreen);
        if (user.Color == MainCore.MainEngine.Color.White)
        {
            chessBoard.Draw(_spriteBatch);
        }
        else chessBoard.DrawInverted(_spriteBatch);
        //loginScreen.Draw(_spriteBatch);
        _spriteBatch.End();
        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}