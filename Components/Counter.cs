using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameAttempt1.Components
{
    public sealed class Counter<T> : TextComponent
    {
        public T Value { get; set; }
        private Func<T> _updater;

        public Counter(SpriteFont font, Vector2 position, string content, T defaultValue, Func<T> updater) : base(font, position, content)
        {
            Value = defaultValue;
            _content += " : " + Value;
            _updater = updater;
        }

        public Counter(SpriteFont font, Vector2 position, string content, Color color, T defaultValue, Func<T> updater)
            : base(font, position, content, color)
        {
            Value = defaultValue;
            _content += " : " + Value;
            _updater = updater;
        }

        public override void Update(GameTime gameTime)
        {
            var newValue = _updater.Invoke();
            if (Value.Equals(newValue)) return;
            Value = newValue;
            _content = _content[..(_content.LastIndexOf(':'))] + ": " + Value;
        }
    }
}
