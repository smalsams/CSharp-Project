using GameAttempt1.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Entities
{
    public abstract class BaseEnemy : IEntity
    {
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public Vector2 Position { get; set; }
        public Rectangle BoundingRectangle { get; set; }
        protected SpriteStateProcessor _spriteStateProcessor { get; set; }
        public abstract void Die();
        public abstract void Despawn();

        public int Health { get; set; }
    }
}
