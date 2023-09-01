using SamSer.Control;
using SamSer.Serialization;
using SamSer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json.Serialization;

namespace SamSer.Entities
{
    [JsonConverter(typeof(EntityConverter))]
    public class FlameEnemy : BaseEnemy
    {
        public bool OnPlatform { get; private set; }
        public override RectangleF BoundingBox => new(Position.X - 5f, Position.Y -5f, 27, 32);


        public override void Despawn()
        {
            throw new NotImplementedException();
        }

        public override void Die()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (InSight)
            {
                SpriteStateProcessor.Draw(spriteBatch, Position, Direction == Control.GameDirection.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            }
        }

        public override string GetTextureName(JObject jsonObject)
        {
            return "messy";
        }

        public override void LoadTexture(Texture2D texture)
        {
            var singleTexture = new Size(27, 32);
            Width = singleTexture.Width;
            Height = singleTexture.Height;
            SpriteStateProcessor.AddState("Idle", new HorizontalAnimation(texture, 8, new Point(10, 10), singleTexture));
            SpriteStateProcessor.ChangeCurrent("Idle");
            SpriteStateProcessor.Current.Animate();
        }


        public override void Update(GameTime gameTime)
        {
            if (InSight)
            {
                SpriteStateProcessor.Update(gameTime);
                Position += Velocity;
                if (!OnPlatform)
                {
                    Velocity = new Vector2(Velocity.X, 10f);
                }
                else
                {
                    Velocity = new Vector2(Velocity.X, 0);
                }
                OnPlatform = false;
                if (Position.Y > 960)
                {
                    InSight = false;
                }
            }
            else
            {
                Position = new Vector2(0, 0);

            }
        }

        public override void CollisionX(RectangleF rectangle)
        {
            if (BoundingBox.Bottom - Height < rectangle.Top || BoundingBox.Top + Height > rectangle.Bottom)
            {
                return;
            }
            if ((BoundingBox.Right - 5f > rectangle.Left && BoundingBox.Right < rectangle.Right)
                || (BoundingBox.Left + 5f < rectangle.Right && BoundingBox.Left > rectangle.Left) || (BoundingBox.Left < 0))
            {
                switch (Direction)
                {
                    case GameDirection.Left:
                        Direction = GameDirection.Right;
                        break;
                    case GameDirection.Right:
                        Direction = GameDirection.Left;
                        break;
                    default:
                        break;
                }
                Velocity = new Vector2(-Velocity.X, Velocity.Y);

            }
        }

        public override void CollisionY(RectangleF rectangle)
        {
            if (BoundingBox.Right < rectangle.Left || BoundingBox.Left > rectangle.Right)
            {
                return;
            }
            if (BoundingBox.Bottom - rectangle.Top <= Height && BoundingBox.Bottom < rectangle.Bottom && BoundingBox.Bottom > rectangle.Top)
            {
                OnPlatform = true;
            }
        }
    }
}
