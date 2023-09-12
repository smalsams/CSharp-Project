using SamSer.Control;
using SamSer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace SamSer.Entities
{
    public class FlameEnemy : BaseEnemy
    {

        /// <remarks>The bounds/hitboxes of the entity</remarks>
        public override RectangleF BoundingBox => new(Position.X - 5f, Position.Y - 5f, 27, 32);

        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (InSight)
            {
                SpriteStateProcessor.Draw(spriteBatch, Position, Direction == Control.GameDirection.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            }
        }
        /// <inheritdoc/>
        public override string GetTextureName(JObject jsonObject)
        {
            return "messy";
        }
        /// <inheritdoc/>
        public override void LoadTexture(Texture2D texture)
        {
            var singleTexture = new Size(27, 32);
            Width = singleTexture.Width;
            Height = singleTexture.Height;
            SpriteStateProcessor.AddState("Idle", new HorizontalAnimation(texture, 8, new Point(10, 10), singleTexture,3));
            SpriteStateProcessor.ChangeCurrent("Idle");
            SpriteStateProcessor.Current.Animate();
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (Paused) { return; }
            if (InSight)
            {
                Position += Velocity;
                SpriteStateProcessor.Update(gameTime);
                Velocity = !OnPlatform ? new Vector2(Velocity.X, 5f) : new Vector2(Velocity.X, 0);
            }
            OnPlatform = false;
        }
        /// <inheritdoc/>
        public override void CollisionX(RectangleF rectangle)
        {
            if (BoundingBox.Bottom -Height < rectangle.Top || BoundingBox.Top> rectangle.Bottom)
            {
                return;
            }

            if ((!(BoundingBox.Right > rectangle.Left) || !(BoundingBox.Right < rectangle.Right))
                && (!(BoundingBox.Left < rectangle.Right) || !(BoundingBox.Left > rectangle.Left)) &&
                (!(BoundingBox.Left < 0))) return;
            Direction = Direction switch
            {
                GameDirection.Left => GameDirection.Right,
                GameDirection.Right => GameDirection.Left,
                _ => Direction
            };
            Velocity = new Vector2(-Velocity.X, Velocity.Y);
        }
        /// <inheritdoc/>
        public override void CollisionY(RectangleF rectangle)
        {
            if (BoundingBox.Right < rectangle.Left || BoundingBox.Left > rectangle.Right)
            {
                return;
            }
            if (BoundingBox.Bottom > rectangle.Top && BoundingBox.Bottom - rectangle.Top < 15f)
            {
                OnPlatform = true;
            }
        }
    }
}
