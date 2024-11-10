using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FogOfWarChess.GUI
{
    abstract class OnScreenObject
    {
        protected Vector2 Position { get; set; }
        protected int Width { get; set; }
        protected int Height { get; set; }
        protected string Text { get; set; }
        protected Texture2D Texture { get; set; }

        public OnScreenObject(Vector2 position, int width, int height, string text, Texture2D texture)
        {
            Position = position;
            Width = width;
            Height = height;
            Text = text;
            Texture = texture;
        }

        public bool IsMouseOver(MouseState mouseState)
        {
            Rectangle bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            return bounds.Contains(mouseState.Position);
        }

        public virtual void LoadTexture(ContentManager content, string texturePath)
        {
            Texture = content.Load<Texture2D>(texturePath);
        }

        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color.White);
            }
            if (!string.IsNullOrEmpty(Text))
            {
                Vector2 textSize = font.MeasureString(Text);
                Vector2 textPosition = Position + new Vector2((Width - textSize.X) / 2, (Height - textSize.Y) / 2);
                spriteBatch.DrawString(font, Text, textPosition, Color.Black);
            }
        }
    }

    class Button : OnScreenObject
    {
        public bool IsClicked { get; private set; }

        public Button(Vector2 position, int width, int height, string text, Texture2D texture)
            : base(position, width, height, text, texture) { }

        public void Update(MouseState mouseState)
        {
            if (IsMouseOver(mouseState) && mouseState.LeftButton == ButtonState.Pressed)
            {
                IsClicked = true;
            }
            else
            {
                IsClicked = false;
            }
        }
    }

    class Checkbox : OnScreenObject
    {
        public bool IsChecked { get; private set; }
        private Texture2D checkedTexture;
        private Texture2D uncheckedTexture;

        public Checkbox(Vector2 position, int width, int height, Texture2D uncheckedTexture, Texture2D checkedTexture)
            : base(position, width, height, null, uncheckedTexture)
        {
            this.uncheckedTexture = uncheckedTexture;
            this.checkedTexture = checkedTexture;
            IsChecked = false;
        }

        public void Update(MouseState mouseState)
        {
            if (IsMouseOver(mouseState) && mouseState.LeftButton == ButtonState.Pressed)
            {
                IsChecked = !IsChecked;
                Texture = IsChecked ? checkedTexture : uncheckedTexture;
            }
        }
    }

    // Textbox class
    class Textbox : OnScreenObject
    {
        public string InputText { get; private set; } = "";
        private bool isSelected;

        public Textbox(Vector2 position, int width, int height, Texture2D texture)
            : base(position, width, height, null, texture) { }

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            if (IsMouseOver(mouseState) && mouseState.LeftButton == ButtonState.Pressed)
            {
                isSelected = true;
            }
            else if (mouseState.LeftButton == ButtonState.Pressed)
            {
                isSelected = false;
            }

            if (isSelected)
            {
                foreach (var key in keyboardState.GetPressedKeys())
                {
                    if (key == Keys.Back && InputText.Length > 0)
                    {
                        InputText = InputText[..^1];
                    }
                    else if (key != Keys.Back)
                    {
                        InputText += key.ToString();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            base.Draw(spriteBatch, font);
            if (!string.IsNullOrEmpty(InputText))
            {
                spriteBatch.DrawString(font, InputText, Position + new Vector2(5, 5), Color.Black);
            }
        }
    }
}
