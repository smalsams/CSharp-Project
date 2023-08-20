using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAttempt1.Sprites
{
    public class Animation
    {
        public const int SCALE = 3;
        public const int FPS = 10;
        public AnimationState State { get; private set; } = AnimationState.Cancelled;
        public double Time { get; private set; } = 0;
        public double TotalTime => (double)SpriteLength / FPS;
        public int CurrentYCoordinate => (int)(Time * FPS);
        public Texture2D Texture { get; }
        public int SpriteLength { get; }
        public int SpriteWidth { get; }
        public int SpriteHeight { get; }
        public (int, int) Origin { get; }
        public Vector2 SpriteOrigin { get; }
        public Animation(Texture2D texture, int count, (int, int) origin, (int, int) dimensions)
        {
            SpriteLength = count;
            Texture = texture;
            SpriteWidth = dimensions.Item1;
            SpriteHeight = dimensions.Item2;
            Origin = origin;
            SpriteOrigin = new Vector2(SpriteWidth/2, SpriteHeight/2);
        }
        public void Animate()
        {
            State = AnimationState.Animated;
        }
        public void Cancel()
        {
            State = AnimationState.Cancelled;
            Time = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (State != AnimationState.Animated) return;
            Time += gameTime.ElapsedGameTime.TotalSeconds;

            if (Time > TotalTime)
            {
                Time = 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            var drawRect =
                new Rectangle(Origin.Item1, Origin.Item2 + CurrentYCoordinate * (SpriteHeight + Origin.Item2) - 1, SpriteWidth, SpriteHeight);

            spriteBatch.Draw(
                Texture,
                new Rectangle(position.ToPoint(), new Point(SpriteWidth * SCALE, SpriteHeight * SCALE)),
                drawRect,
                Color.White,
                0,
                SpriteOrigin,
                spriteEffects,
                0
                );
        }
    }
}
