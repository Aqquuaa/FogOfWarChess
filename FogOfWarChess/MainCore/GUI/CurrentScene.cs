using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FogOfWarChess.MainCore.MainEngine;

namespace FogOfWarChess.GUI;

enum CurrentSceneEnum
{
    loginScreen = 1,
    menuScreen = 2,
    gameScreen = 3,
    waitingScreen = 4
}

public abstract class CurrentScene
{
    protected bool sceneChanged = false;
    public bool SceneChanged()
    {
        bool currentValue = sceneChanged;
        sceneChanged = false;
        return currentValue;
    }
    public abstract void Update(MouseState mouseState);
    public abstract void Draw(SpriteBatch spriteBatch, SpriteFont font);
    public abstract void Load(ContentManager content);
}

class Login : CurrentScene
{
    private Button loginButton;
    private Button exitButton;
    private bool loginButtonClicked = false;
    public bool LoginButtonClicked
    {
        get { return loginButtonClicked; }
    }
    public Login(GraphicsDevice graphicsDevice)
    {
        int screenWidth = graphicsDevice.Viewport.Width;
        int screenHeight = graphicsDevice.Viewport.Height;

        int buttonWidth = 200;
        int buttonHeight = 50;

        Vector2 loginButtonPosition = new Vector2((screenWidth - buttonWidth) / 2, (screenHeight / 2) - 60);
        Vector2 exitButtonPosition = new Vector2((screenWidth - buttonWidth) / 2, (screenHeight / 2) + 10);

        loginButton = new Button(loginButtonPosition, buttonWidth, buttonHeight, "Login", null);
        exitButton = new Button(exitButtonPosition, buttonWidth, buttonHeight, "Exit", null);
    }

    public override void Load(ContentManager content)
    {
        loginButton.LoadTexture(content, "CoreTextures/Button");
        exitButton.LoadTexture(content, "CoreTextures/Button");
    }
    public override void Update(MouseState mouseState)
    {
        loginButton.Update(mouseState);
        exitButton.Update(mouseState);

        if (loginButton.IsClicked)
        {
            Debug.WriteLine("Login button clicked.");
            loginButtonClicked = true;
            sceneChanged = true;
        }

        if (exitButton.IsClicked)
        {
            Debug.WriteLine("Exit button clicked.");

            Environment.Exit(0);
        }
    }
    public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
    {
        loginButton.Draw(spriteBatch, font);
        exitButton.Draw(spriteBatch, font);
    }
}

class Menu : CurrentScene
{
    private Button startButton;
    private Textbox textbox;
    private string ipAdress = null;
    //private int port;
    private bool startButtonClicked = false;
    public bool StartButtonClicked()
    {
        bool currentValue = startButtonClicked;
        startButtonClicked = false;
        return currentValue;
    }
    public string IpAdress
    {
        get { return ipAdress; }
    }
    public Menu(GraphicsDevice graphicsDevice)
    {
        int screenWidth = graphicsDevice.Viewport.Width;
        int screenHeight = graphicsDevice.Viewport.Height;

        int buttonWidth = 200;
        int buttonHeight = 50;

        int textboxWidth = 200;
        int textboxHeight = 25;

        Vector2 startButtonPosition = new Vector2((screenWidth - buttonWidth) / 2, (screenHeight - buttonHeight) / 2);
        Vector2 startTextBoxPosition = new Vector2((screenWidth - buttonWidth) / 2, (screenHeight - buttonHeight) / 2 + 70);
        startButton = new Button(startButtonPosition, buttonWidth, buttonHeight, "Start Game", null);
        textbox = new Textbox(startTextBoxPosition, textboxWidth, textboxHeight, null);
    }

    public override void Load(ContentManager content)
    {
        startButton.LoadTexture(content, "CoreTextures/Button");
        textbox.LoadTexture(content, "CoreTextures/Button");
    }

    public override void Update(MouseState mouseState)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        startButton.Update(mouseState);
        textbox.Update(mouseState, keyboardState);
        if (startButton.IsClicked)
        {
            ipAdress = textbox.InputText;
            startButtonClicked = true;
            sceneChanged = true;
            Debug.WriteLine("Start button clicked.");
        }

    }

    public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
    {
        startButton.Draw(spriteBatch, font);
        textbox.Draw(spriteBatch, font);
    }
}

class GameScreen : CurrentScene
{
    private ChessBoard chessBoard;
    private User user;
    private Checkbox checkBox;
    private bool checkboxBool = true;
    public bool CheckBoxClicked
    {
        get { return checkboxBool; }
    }
    public GameScreen(GraphicsDevice graphicsDevice, ChessBoard chessBoard, User user)
    {
        int screenWidth = graphicsDevice.Viewport.Width;
        int screenHeight = graphicsDevice.Viewport.Height;

        int checkboxWidth = 25;
        int checkboxHeight = 25;

        Vector2 startCheckBoxPosition = new Vector2((screenWidth - checkboxWidth) - 5, (checkboxHeight) + 5);

        checkBox = new Checkbox(startCheckBoxPosition, checkboxWidth, checkboxHeight, null, null);

        this.chessBoard = chessBoard;
        this.user = user;
    }

    public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
    {
        checkBox.Draw(spriteBatch, font);
        if (user.Color == MainCore.MainEngine.Color.White)
        {
            chessBoard.Draw(spriteBatch);
        }
        else chessBoard.DrawInverted(spriteBatch);
    }

    public override void Load(ContentManager content)
    {
        checkBox.LoadTexture(content, null);
    }

    public override void Update(MouseState mouseState)
    {
        checkBox.Update(mouseState);
        KeyboardState keyboardState = Keyboard.GetState();
        //user.GetUserInputForGame(keyboardState, mouseState, chessBoard);
        checkboxBool = !checkBox.IsChecked;
    }
}