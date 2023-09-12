using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SamSer.Components
{
    /// <summary>
    /// Defines a clickable area on the screen implemented with an event as the trigger behind it
    /// </summary>
    public class Button : Component
    {
        #region Fields and Properties
        protected MouseState CurrentMouseState;
        protected MouseState PreviousMouseState;
        protected Keys KeyInvoker;
        protected KeyboardState PreviousKeyboardState;
        protected KeyboardState CurrentKeyboardState;
        private readonly Texture2D _texture;
        private readonly SpriteFont _font;
        protected bool Hovered;

        /// <summary>
        /// User defined event invoked at pressing the <see cref="Button"/>
        /// </summary>
        public EventHandler ButtonPress;
        public bool Pressed => Hovered &&
                               CurrentMouseState.LeftButton == ButtonState.Released &&
                               PreviousMouseState.LeftButton == ButtonState.Pressed;
        /// <remarks>
        /// Color of the <see cref="Content"/> text.
        /// </remarks>
        public Color TextColor { get; }
        public Vector2 Position { get; set; }
        /// <remarks>Hitbox of the <see cref="Button"/></remarks>
        public Rectangle Rectangle => new((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
        /// <remarks>Text for the <see cref="Button"/></remarks>
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


        ///<inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Hovered ? Color.MediumPurple : Color.White;
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

        /// <summary>
        /// Synchronizes the keyboard and mouse states with real-time inputs
        /// </summary>
        public void UpdateInput()
        {
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }
        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            UpdateInput();
            var mouseHitbox = new Rectangle(CurrentMouseState.X, CurrentMouseState.Y, 1, 1);
            Hovered = mouseHitbox.Intersects(Rectangle);
            if (Pressed || (CurrentKeyboardState.IsKeyDown(KeyInvoker) && !PreviousKeyboardState.IsKeyDown(KeyInvoker)))
            {
                ButtonPress?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Allows for buttons being invokable by keyboard events.
        /// </summary>
        /// <param name="key">Any key on the keyboard</param>
        public void AddKeyboardInvoker(Keys key)
        {
            KeyInvoker = key;
        }
    }
}
