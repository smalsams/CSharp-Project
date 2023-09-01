using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SamSer.Sprites
{
    public class HorizontalAnimation : AnimationBase
    {
        private int _currentXCoordinate => (int)(Time * FPS);
        public HorizontalAnimation(Texture2D texture, int spriteAnimationCount, Point textureOrigin, Point spriteDimensions, int entityScale = 1)
            : base(texture, spriteAnimationCount, textureOrigin, spriteDimensions, entityScale)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            var drawRectangle = new Rectangle(Origin.X + _currentXCoordinate * (SpriteWidth + Origin.X), Origin.Y, SpriteWidth, SpriteHeight);
            spriteBatch.Draw(
                Texture,
                new Rectangle(position.ToPoint(), new Point(SpriteWidth * Scale, SpriteHeight * Scale)),
                drawRectangle,
                Color.White,
                0,
                MapOrigin,
                spriteEffects,
                0
                );
        }

        public override void Update(GameTime gameTime)
        {
            Time += gameTime.ElapsedGameTime.TotalSeconds;

            if (Time > TotalTime)
            {
                Time = 0;
            }
        }
    }
}
