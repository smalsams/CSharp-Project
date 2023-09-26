using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SamSer.Components
{
    /// <summary>
    /// Adds a context to the basic button, can be used as serial buttons, each with a different (or same) context.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SerialButton<T> : Button
    {
        /// <remarks>
        /// User defined event invoked after pressing the button
        /// </remarks>
        public new EventHandler<T> ButtonPress { get; set; }
        /// <remarks>
        /// Context of the button, can act as a sort of unique identifier
        /// </remarks>
        private readonly T _context;
        public SerialButton(Texture2D texture, SpriteFont font, Vector2 position, string content, T context) : base(texture, font, position, content)
        {
            _context = context;
        }
        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            UpdateInput();
            var mouseHitbox = new Rectangle(CurrentMouseState.X, CurrentMouseState.Y, 1, 1);
            Hovered = mouseHitbox.Intersects(Rectangle);
            if (Pressed || (CurrentKeyboardState.IsKeyDown(KeyInvoker) && !PreviousKeyboardState.IsKeyDown(KeyInvoker)))
            {
                ButtonPress?.Invoke(this, _context);
            }
        }

    }
}
