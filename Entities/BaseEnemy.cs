using GameAttempt1.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json.Linq;
using System;

namespace GameAttempt1.Entities
{
    public abstract class BaseEnemy : IEntity
    {
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public Vector2 Position { get; set; }
        public RectangleF BoundingBox { get; }
        protected SpriteStateProcessor _spriteStateProcessor { get; set; }
        public abstract void Die();
        public abstract void Despawn();

        public abstract string GetTextureName(JObject jsonObject);
        public abstract void LoadTexture(Texture2D texture);

        public int Health { get; set; }
        public EventHandler PlayerCollisionEvent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
