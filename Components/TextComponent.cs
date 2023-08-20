using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAttempt1.Components
{
    public class TextComponent : Component
    {
        protected string _content;
        private Vector2 _position;
        private SpriteFont _font;
        private Color _color;

        public TextComponent(SpriteFont font, Vector2 position, string content)
        {
            _font = font;
            _position = position;
            _content = content;
            _color = Color.Black;
        }

        public TextComponent(SpriteFont font, Vector2 position, string content, Color color)
        {
            _font = font;
            _position = position;
            _content = content;
            _color = color;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _content, _position, _color);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
