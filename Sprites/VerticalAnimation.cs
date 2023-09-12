using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SamSer.Sprites
{
    /// <inheritdoc/>
    /// <remarks>Animation that changes sprite offset on Y-axis</remarks>
    public class VerticalAnimation : AnimationBase
    {
        public int CurrentYCoordinate => (int)(Time * Fps);
        public VerticalAnimation(Texture2D texture, int count, Point origin, Point dimensions, int scale = 1) :
            base(texture, count, origin, dimensions, scale)
        {
        }
        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (State != AnimationState.Animated) return;
            Time += gameTime.ElapsedGameTime.TotalSeconds;

            if (Time > TotalTime)
            {
                Time = 0;
            }
        }
        /// <inheritdoc/>
        public override void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            var drawRect =
                new Rectangle(Origin.X, Origin.Y + CurrentYCoordinate * (SpriteHeight + Origin.Y) - 1, SpriteWidth, SpriteHeight);

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
