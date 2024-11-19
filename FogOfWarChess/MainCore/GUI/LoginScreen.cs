using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace FogOfWarChess.GUI
{
    public class LoginScreen
    {
        private Textbox usernameTextbox;
        private Textbox passwordTextbox;
        private Button loginButton;

        private SpriteFont font;
        private Texture2D textboxTexture;
        private Texture2D buttonTexture;

        public LoginScreen()
        {
            //usernameTextbox = new Textbox(new Vector2(50, 50), 100, 20, null);
            //passwordTextbox = new Textbox(new Vector2(position.X, position.Y + textboxHeight + 10), textboxWidth, textboxHeight, null);
            //loginButton = new Button(new Vector2(30, 30), 50, 20, "Login", buttonTexture);
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Font/myfont");
            //textboxTexture = content.Load<Texture2D>("TextboxTexture"); 
            buttonTexture = content.Load<Texture2D>("CoreTextures/Button");
            loginButton = new Button(new Vector2(30, 130), 50, 20, "Login", buttonTexture);
        }

        public CurrentSceneEnum Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();

            // Update each textbox and button
            //usernameTextbox.Update(mouseState, keyboardState);
            //passwordTextbox.Update(mouseState, keyboardState);
            loginButton.Update(mouseState);

            // Handle the login button click
            if (loginButton.IsClicked)
            {
                return CurrentSceneEnum.GameScreen;
                //string username = usernameTextbox.InputText;
                //string password = passwordTextbox.InputText;

                // Process login (e.g., validate username and password)
                // Add login logic here (e.g., call authentication method or display error message)
            }
            return CurrentSceneEnum.LoginScreen;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the username textbox, password textbox, and login button
            //usernameTextbox.Draw(spriteBatch, font);
            //passwordTextbox.Draw(spriteBatch, font);
            loginButton.Draw(spriteBatch, font);

            // Optionally, draw labels for the textboxes
            //spriteBatch.DrawString(font, "Username:", usernameTextbox.Position + new Vector2(-70, 5), Color.Black);
            //spriteBatch.DrawString(font, "Password:", passwordTextbox.Position + new Vector2(-70, 5), Color.Black);
        }
    }
}
