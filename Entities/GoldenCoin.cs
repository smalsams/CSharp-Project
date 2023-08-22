using GameAttempt1.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Entities
{
    public class Coin : IEntity
    {
        private SpriteStateProcessor _spriteStateProcessor;
        private Vector2 _position;
        public Coin(ContentManager content, Vector2 position, CoinType type)
        {
            var texture = content.Load<Texture2D>($"Sprites/coin_{type}");
            _position = position;
            _spriteStateProcessor = new SpriteStateProcessor();
            _spriteStateProcessor.AddState("i", new HorizontalAnimation(texture, 8, new Point(0, 0), new Point(32, 32)));
            _spriteStateProcessor.ChangeCurrent("i");
            _spriteStateProcessor.Current?.Animate();
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _spriteStateProcessor.Draw(spriteBatch, _position, SpriteEffects.None);
        }
    }
}
