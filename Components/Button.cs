﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SamSer.Components
{
    public class Button : Component
    {
        #region Fields and Properties
        protected MouseState _currentMouseState;
        protected MouseState _previousMouseState;
        protected Keys _keyInvoker;
        protected KeyboardState _previousKeyboardState;
        protected KeyboardState _currentKeyboardState;
        private readonly Texture2D _texture;
        private readonly SpriteFont _font;
        protected bool _hovered;

        public EventHandler ButtonPress;
        public bool Deactivated { get; set; }
        public bool Pressed => _hovered &&
                               _currentMouseState.LeftButton == ButtonState.Released &&
                               _previousMouseState.LeftButton == ButtonState.Pressed;
        public Color TextColor { get; }
        public Vector2 Position { get; set; }

        public Rectangle Rectangle => new((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
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
