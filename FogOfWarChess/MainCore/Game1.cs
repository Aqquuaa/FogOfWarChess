﻿using FogOfWarChess.MainCore.MainEngine;
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

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        chessBoard.Draw(_spriteBatch);
        _spriteBatch.End();
        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
