using SamSer.Control;
using SamSer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SamSer.Entities
{
    /// <summary>
    /// Defines an entity that kills player upon intersection with its texture
    /// </summary>
    public abstract class BaseEnemy : IEntity
    {
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Rotation { get; set; }
        public abstract RectangleF BoundingBox { get; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        protected SpriteStateProcessor SpriteStateProcessor { get; set; }
        public bool InSight { get; set; }
        public GameDirection Direction { get; set; }
        public abstract void Die();
        public abstract void Despawn();
        public abstract string GetTextureName(JObject jsonObject);
        public abstract void LoadTexture(Texture2D texture);
        [JsonConstructor]
        public BaseEnemy()
        {
            SpriteStateProcessor = new SpriteStateProcessor();
        }

        public BaseEnemy(Texture2D texture)
        {
            Width = texture.Width;
            Height = texture.Height;
            SpriteStateProcessor = new SpriteStateProcessor();
        }

        public abstract void CollisionX(RectangleF rectangle);
        public abstract void CollisionY(RectangleF rectangle);

        public int Health { get; set; }
        public EventHandler PlayerCollisionEvent { get; set; }
        public int Id { get; set; }
    }
}
