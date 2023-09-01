using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SamSer.Components
{
    public class SerialButton<T> : Button
    {
        public new EventHandler<T> ButtonPress { get; set; }
        private T _context;
        public SerialButton(Texture2D texture, SpriteFont font, Vector2 position, string content, T context) : base(texture, font, position, content)
        {
            _context = context;
        }
        public override void Update(GameTime gameTime)
        {
            UpdateInput();
            var mouseHitbox = new Rectangle(_currentMouseState.X, _currentMouseState.Y, 1, 1);
            _hovered = mouseHitbox.Intersects(Rectangle);
            if (Pressed || (_currentKeyboardState.IsKeyDown(_keyInvoker) && !_previousKeyboardState.IsKeyDown(_keyInvoker)))
            {
                ButtonPress?.Invoke(this, _context);
            }
        }

    }
}
