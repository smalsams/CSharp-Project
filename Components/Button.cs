using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace GameAttempt1.Components
{
    public class Button : Component
    {
        #region Fields and Properties
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private Keys _keyInvoker;
        private KeyboardState _previousKeyboardState;
        private KeyboardState _currentKeyboardState;
        private readonly Texture2D _texture;
        private readonly SpriteFont _font;
        private bool _hovered;

        public EventHandler ButtonPress;

        public bool Pressed => _hovered &&
                               _currentMouseState.LeftButton == ButtonState.Released &&
                               _previousMouseState.LeftButton == ButtonState.Pressed;
        public Color TextColor { get; }
        public Vector2 Position { get; set; }

        public Rectangle Rectangle => new ((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
        public string Content { get; }
        #endregion

        #region Constructors

        public Button(Texture2D texture, SpriteFont font, Vector2 position, string content)
        {
            _texture = texture;
            Position = position;
            Content = content;
            _font = font;
            TextColor = Color.Black;
        }
        #endregion

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = _hovered ? Color.MediumPurple : Color.White;
            spriteBatch.Draw(_texture, Rectangle, color);
            if (!string.IsNullOrEmpty(Content))
            {
                spriteBatch.DrawString(_font,
                    Content,
                    new Vector2(Rectangle.X + Rectangle.Width / 2 - _font.MeasureString(Content).X / 2,
                        Rectangle.Y + Rectangle.Height / 2 - _font.MeasureString(Content).Y / 2),
                    TextColor);
            }
        }


        public void UpdateInput()
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
        }
        public override void Update(GameTime gameTime)
        {
            UpdateInput();
            var mouseHitbox = new Rectangle(_currentMouseState.X, _currentMouseState.Y, 1, 1);
            _hovered = mouseHitbox.Intersects(Rectangle);
            if (Pressed || (_currentKeyboardState.IsKeyDown(_keyInvoker) && !_previousKeyboardState.IsKeyDown(_keyInvoker)))
            {
                ButtonPress?.Invoke(this, EventArgs.Empty);
            }
        }

        public void AddKeyboardInvoker(Keys key)
        {
            _keyInvoker = key;
        }
    }
}
