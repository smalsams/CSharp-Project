using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SamSer.Components
{
    /// <summary>
    /// Generic implementation for Text area with an user defined updater method assigned to it
    /// </summary>
    /// <typeparam name="T">Type of the counted value, should have a meaningful string conversion</typeparam>
    public sealed class Counter<T> : TextComponent
    {
        /// <remarks>Value of type <see cref="T"/> to be displayed as a counting object after the text area</remarks>
        public T Value { get; set; }
        /// <summary>
        /// User defined method, which allows for generic updating of the <see cref="Value"/>
        /// </summary>
        private readonly Func<T> _updater;

        public Counter(SpriteFont font, Vector2 position, string content, T defaultValue, Func<T> updater) : base(font, position, content)
        {
            Value = defaultValue;
            Content += " : " + Value;
            _updater = updater;
        }

        public Counter(SpriteFont font, Vector2 position, string content, Color color, T defaultValue, Func<T> updater)
            : base(font, position, content, color)
        {
            Value = defaultValue;
            Content += " : " + Value;
            _updater = updater;
        }
        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            var newValue = _updater.Invoke();
            if (Value.Equals(newValue)) return;
            Value = newValue;
            Content = Content[..Content.LastIndexOf(':')] + ": " + Value;
        }
    }
}
