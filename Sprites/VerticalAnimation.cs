using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SamSer.Sprites
{
    public class VerticalAnimation : AnimationBase
    {
        public int _currentYCoordinate => (int)(Time * FPS);
        public VerticalAnimation(Texture2D texture, int count, Point origin, Point dimensions, int scale = 1) :
            base(texture, count, origin, dimensions, scale)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (State != AnimationState.Animated) return;
            Time += gameTime.ElapsedGameTime.TotalSeconds;

            if (Time > TotalTime)
            {
                Time = 0;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            var drawRect =
                new Rectangle(Origin.X, Origin.Y + _currentYCoordinate * (SpriteHeight + Origin.Y) - 1, SpriteWidth, SpriteHeight);

            spriteBatch.Draw(
                Texture,
                new Rectangle(position.ToPoint(), new Point(SpriteWidth * Scale, SpriteHeight * Scale)),
                drawRect,
                Color.White,
                0,
                MapOrigin,
                spriteEffects,
                0
                );
        }
    }
}
