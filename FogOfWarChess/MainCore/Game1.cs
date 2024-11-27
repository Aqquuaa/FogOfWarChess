using FogOfWarChess.GUI;
using FogOfWarChess.MainCore.MainEngine;
using FogOfWarChess.NetEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace FogOfWarChess;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private ChessBoard chessBoard;
    private User user;
    private CurrentScene _currentScene;
    private Login loginScene;
    private Menu menuScene;
    private GameScreen gameScene;
    private SpriteFont font;
    private CurrentSceneEnum currentSceneEnum = CurrentSceneEnum.loginScreen;
    private ClientConnection clientConnection;
    private string userColor;
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
        clientConnection = new ClientConnection(OnMoveReceived);


        WindowSizeFind(chessBoard.boardSize);
        InitScreens();
        base.Initialize();
    }

    protected void InitScreens()
    {
        loginScene = new Login(GraphicsDevice);
        menuScene = new Menu(GraphicsDevice);
        gameScene = new GameScreen(GraphicsDevice, chessBoard, user);
        _currentScene = loginScene;
    }

    private void ChangeScreen(CurrentSceneEnum sceneChange)
    {
        switch (sceneChange)
        {
            case CurrentSceneEnum.loginScreen:
                {
                    break;
                }

            case CurrentSceneEnum.menuScreen:
                {
                    _currentScene = menuScene;
                    break;
                }
            case CurrentSceneEnum.gameScreen:
                {
                    _currentScene = gameScene;
                    break;
                }

        }
    }
    protected void WindowSizeFind(int sizeOfBoard)
    {
        int Width = sizeOfBoard * 40;
        int Height = sizeOfBoard * 40;
        WindowSizeApply(Width, Height);
    }

    protected void WindowSizeApply(int Width, int Height)
    {
        _graphics.PreferredBackBufferWidth = Width+35;
        _graphics.PreferredBackBufferHeight = Height;
        _graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        font = Content.Load<SpriteFont>("Font/myfont");
        loginScene.Load(Content);
        menuScene.Load(Content);
        gameScene.Load(Content);
        LoadMusic();
        chessBoard.LoadTexture(Content);
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
            Exit();
        // TODO: Add your update logic here

        if (_currentScene.SceneChanged())
        {
            switch (currentSceneEnum)
            {
                case CurrentSceneEnum.loginScreen:
                    if (loginScene.LoginButtonClicked)
                    {
                        currentSceneEnum = CurrentSceneEnum.menuScreen;
                    }
                    break;

                case CurrentSceneEnum.menuScreen:

                    if (menuScene.StartButtonClicked())
                    {
                        connectionTask = Task.Run(() =>
                        {
                            try
                            {
                                if (clientConnection.InitConnect(menuScene.IpAdress, 11111))
                                {
                                isMyTurn = clientConnection.IsPlayerTurn;
                                string userColor = ClientConnection.UserColor;
                                connectionSuccessful = true;
                                user.InitUser(chessBoard, userColor);
                                Debug.WriteLine("Connected");
                                currentSceneEnum = CurrentSceneEnum.gameScreen;
                                ChangeScreen(currentSceneEnum);
                                }
                                else
                                {
                                    Debug.WriteLine("Server with such Ip adress is not responding!");
                                }
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
                    }
                    break;

                case CurrentSceneEnum.gameScreen:
                    if (gameScene.CheckBoxClicked)
                    {
                        MediaPlayer.Volume = 1f;
                    }
                    else
                    {
                        MediaPlayer.Volume = 0f;
                    }
                    break;
            }

            ChangeScreen(currentSceneEnum);
        }
        if (currentSceneEnum == CurrentSceneEnum.gameScreen && isMyTurn)
        {
            HandleInput(gameTime);
        }
        if (currentSceneEnum == CurrentSceneEnum.gameScreen && connectionSuccessful)
        {

        }
        _currentScene.Update(Mouse.GetState());

        if (gameScene.CheckBoxClicked == true)
        {
            MediaPlayer.Volume = 1f;
        }
        else
        {
            MediaPlayer.Volume = 0f;
        }

        base.Update(gameTime);
    }
    private void OnMoveReceived(NormalMove move)
    {
        chessBoard.ApplyMove(move);
        user.CallFog(chessBoard);
        isMyTurn = true;
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
            Debug.WriteLine("move");
        }
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null);
        _currentScene.Draw(_spriteBatch, font);

        _spriteBatch.End();
        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}