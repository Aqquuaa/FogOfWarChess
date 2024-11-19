using FogOfWarChess.MainCore.MainEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FogOfWarChess.GUI;

public enum CurrentSceneEnum
{
    LoginScreen,
    MainMenuScreen,
    SettingsScreen,
    GameScreen
}

public class CurrentScene
{
    private CurrentSceneEnum currentScene;
    public CurrentScene(CurrentSceneEnum initialScene)
    {
        currentScene = initialScene;
    }

    public void SetScene(CurrentSceneEnum newScene)
    {
        currentScene = newScene;
    }
    public void SceneDraw(SpriteBatch _spriteBatch, LoginScreen loginScreen)
    {
        switch (currentScene)
        {
            case CurrentSceneEnum.LoginScreen:
            {
                loginScreen.Draw(_spriteBatch);
                break;
            }
            case CurrentSceneEnum.MainMenuScreen:
            {
                break;
            }
            case CurrentSceneEnum.SettingsScreen:
            {
                break;
            }
            case CurrentSceneEnum.GameScreen:
            {
                /*if (user.Color == MainCore.MainEngine.Color.White)
                {
                    chessBoard.Draw(_spriteBatch);
                }
                else chessBoard.DrawInverted(_spriteBatch);*/
                    break;
            }
        }
    }

    public void SceneUpdate(LoginScreen loginScreen, GameTime _gameTime)
    {
        switch (currentScene)
    {
        case CurrentSceneEnum.LoginScreen:
        {
            loginScreen.Update(_gameTime);
            break;
        }
        case CurrentSceneEnum.MainMenuScreen:
        {
            break;
        }
        case CurrentSceneEnum.SettingsScreen:
        {
            break;
        }
        case CurrentSceneEnum.GameScreen:
        {
            break;
        }
    }
    }

}