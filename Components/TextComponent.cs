using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SamSer.Components{

    /// <summary>
    /// Basic text area on screen, can be used as a label or a counter.
    /// </summary>
    public class TextComponent : Component
    {
        protected string Content;
        private readonly Vector2 _position;
        private readonly SpriteFont _font;
        private readonly Color _color;

        public TextComponent(SpriteFont font, Vector2 position, string content)
        {
            _font = font;
            _position = position;
            Content = content;
            _color = Color.Black;
        }

        public TextComponent(SpriteFont font, Vector2 position, string content, Color color)
        {
            _font = font;
            _position = position;
            Content = content;
            _color = color;
        }
        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, Content, _position, _color);
        }
        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {

        }
    }
}
