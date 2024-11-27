using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;

namespace FogOfWarChess.GUI
{
    abstract class OnScreenObject
    {
        protected ButtonState previousButtonState;
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
            if (IsMouseOver(mouseState) &&
                mouseState.LeftButton == ButtonState.Released &&
                previousButtonState == ButtonState.Pressed)
            {
                IsClicked = true;
            }
            else
            {
                IsClicked = false;
            }

            previousButtonState = mouseState.LeftButton;
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

        public override void LoadTexture(ContentManager content, string texturePath)
        {
            checkedTexture = content.Load<Texture2D>("CoreTextures/CCheckBox");
            uncheckedTexture = content.Load<Texture2D>("CoreTextures/UCheckBox");
            Texture = uncheckedTexture;
        }

        public void Update(MouseState mouseState)
        {
            if (IsMouseOver(mouseState) &&
                mouseState.LeftButton == ButtonState.Released &&
                previousButtonState == ButtonState.Pressed)
            {
                IsChecked = !IsChecked; 
                Texture = IsChecked ? checkedTexture : uncheckedTexture; 
            }

            previousButtonState = mouseState.LeftButton;
        }
    }

    class Textbox : OnScreenObject
    {
        public string InputText { get; private set; } = "";
        private bool isSelected;
        private Keys[] previousKeys = Array.Empty<Keys>();

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
                var currentKeys = keyboardState.GetPressedKeys();

                foreach (var key in currentKeys)
                {
                    Debug.WriteLine(key);
                    if (!previousKeys.Contains(key))
                    {
                        if (key == Keys.Back && InputText.Length > 0)
                        {
                            InputText = InputText[..^1];
                        }
                        else if (key >= Keys.D0 && key <= Keys.D9)
                        {
                            InputText += (key - Keys.D0).ToString();
                        }
                        /*else if (key != Keys.Back && key != Keys.LeftShift && key != Keys.Space && key != Keys.CapsLock && key != Keys.Enter && key != Keys.OemPeriod)
                        {
                            InputText += key.ToString(); 
                        }*/
                        else if (key == Keys.OemPeriod)
                        {
                            InputText += '.';
                        }

                    }
                }
                Debug.WriteLine(InputText);
                previousKeys = currentKeys;
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
