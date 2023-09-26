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

        /// <remarks>Determines whether entity is on a platform or in air.</remarks>
        public bool OnPlatform { get; protected set; }

        /// <inheritdoc/>
        public abstract void Update(GameTime gameTime);
        /// <inheritdoc/>
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        /// <inheritdoc/>
        public Vector2 Position { get; set; }
        /// <remarks>Velocity is a vector that determines the speed of the entity in the X and Y axis.</remarks>
        public Vector2 Velocity { get; set; }
        /// <remarks>Rotation is a vector that determines the rotation of the entity in the X and Y axis.</remarks>
        public Vector2 Rotation { get; set; }
        /// <summary>
        /// An imaginary rectangle that specifies the hitboxes of the <see cref="BaseEnemy"/> entity
        /// </summary>
        public abstract RectangleF BoundingBox { get; }
        /// <summary>
        /// Width of the <see cref="BaseEnemy"/>
        /// </summary>
        public int Width { get; protected set; }
        /// <summary>
        /// Height of the <see cref="BaseEnemy"/>
        /// </summary>
        public int Height { get; protected set; }
        /// <summary>
        /// Responsible for the control of animation states given to the <see cref="BaseEnemy"/>
        /// </summary>
        protected SpriteStateProcessor SpriteStateProcessor { get; set; }
        /// <summary>
        /// Specifies whether the entity is visible for the player
        /// </summary>
        public bool InSight { get; set; }
        /// <summary>
        /// The direction the entity is focusing towards
        /// </summary>
        public GameDirection Direction { get; set; }

        /// <inheritdoc/>
        public abstract string GetTextureName(JObject jsonObject);
        /// <inheritdoc/>
        public abstract void LoadTexture(Texture2D texture);
        /// <summary>
        /// Constructs the BaseEnemy from a JSON, used mainly for loading from a file. 
        /// </summary>
        [JsonConstructor]
        public BaseEnemy()
        {
            SpriteStateProcessor = new SpriteStateProcessor();
        }
        /// <summary>
        /// Regular in-code construction for the BaseEnemy class, used mainly for testing.
        /// </summary>
        /// <param name="texture"></param>
        public BaseEnemy(Texture2D texture)
        {
            Width = texture.Width;
            Height = texture.Height;
            SpriteStateProcessor = new SpriteStateProcessor();
        }
        /// <remarks>Specifies whether entity is paused. </remarks>
        public bool Paused { get; set; }
        /// <summary>
        /// Checks if entity collides with a rectangle horizontally
        /// </summary>
        /// <param name="rectangle"></param>
        public abstract void CollisionX(RectangleF rectangle);
        /// <summary>
        /// Checks if entity collides with a rectangle vertically
        /// </summary>
        /// <param name="rectangle"></param>
        public abstract void CollisionY(RectangleF rectangle);

        /// <remarks>Specifies the number of lives left for the enemy entity. </remarks>
        public int Health { get; set; }

        /// <remarks>Collision event between player and entity</remarks>
        public EventHandler PlayerCollisionEvent { get; set; }

        /// <remarks>Specifies the ID of the entity. </remarks>
        public int Id { get; set; }
    }
}
