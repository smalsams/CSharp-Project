using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json.Linq;
using SamSer.Control;
using SamSer.Sprites;
using static SamSer.Utilities.GameUtilities;

namespace SamSer.Entities
{
    public class Zombie : BaseEnemy
    {
        public override RectangleF BoundingBox => new(Position.X - COLLISION_THRESHOLD_X, Position.Y - COLLISION_THRESHOLD_Y, 24, 32);
        public override void Update(GameTime gameTime)
        {
            if (Paused) { return; }
            if (InSight)
            {
                Position += Velocity;
                SpriteStateProcessor.Update(gameTime);
                Velocity = !OnPlatform ? new Vector2(Velocity.X, JUMP_Y_VELOCITY) : new Vector2(Velocity.X, 0);
            }
            OnPlatform = false;
        }

        public override string GetTextureName(JObject jsonObject)
        {
            return "bloody_zombie-SWEN"; 
        }

        public override void LoadTexture(Texture2D texture)
        {
            var singleTexture = new Size(24, 32);
            Width = singleTexture.Width;
            Height = singleTexture.Height;
            SpriteStateProcessor.AddState("Idle",
                new HorizontalAnimation(texture, 3, new Point(0, 32), singleTexture, 2));
            SpriteStateProcessor.ChangeCurrent("Idle");
            SpriteStateProcessor.Current.Animate();
        }

        public override void CollisionX(RectangleF rectangle)
        {
            if (BoundingBox.Bottom - Height < rectangle.Top || BoundingBox.Top > rectangle.Bottom)
            {
                return;
            }

            if ((BoundingBox.Right - BoundingBox.Width/2 > rectangle.Left && BoundingBox.Right < rectangle.Right)
                || (BoundingBox.Left < rectangle.Right && BoundingBox.Left > rectangle.Left) || (BoundingBox.Left < 0 && Velocity.X < 0))
            {
                Direction = Direction switch
                {
                    GameDirection.Left => GameDirection.Right,
                    GameDirection.Right => GameDirection.Left,
                    _ => Direction
                };
                Velocity = new Vector2(-Velocity.X, Velocity.Y);
            }
        }
        /// <inheritdoc/>
        public override void CollisionY(RectangleF rectangle)
        {
            if (BoundingBox.Right < rectangle.Left || BoundingBox.Left > rectangle.Right)
            {
                return;
            }
            if (BoundingBox.Bottom + BoundingBox.Height/2 > rectangle.Top && BoundingBox.Bottom - rectangle.Top < COLLISION_THRESHOLD_Y)
            {
                OnPlatform = true;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (InSight)
            {
                SpriteStateProcessor.Draw(spriteBatch, Position,
                    Direction == GameDirection.Right ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            }
        }
    }
}
