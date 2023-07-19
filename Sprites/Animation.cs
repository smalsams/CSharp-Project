using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Sprites
{
    public class Animation
    {
        public const int FPS = 10;
        public AnimationState State { get; private set; } = AnimationState.Cancelled;
        public double Time { get; private set; } = 0;
        public double TotalTime => (double)SpriteLength / (double)FPS;
        public int CurrentYCoordinate => (int)(Time * FPS);
        public Texture2D Texture { get; private set; }
        public int SpriteLength { get; private set; }
        public int SpriteWidth { get; private set; }
        public int SpriteHeight { get; private set; }
        public (int, int) Origin { get; private set; }
        public Vector2 SpriteOrigin { get; private set; }
        public Animation(Texture2D texture, int count, (int,int) origin, (int,int) dimensions)
        {
            SpriteLength = count;
            Texture = texture;
            SpriteWidth = dimensions.Item1;
            SpriteHeight = dimensions.Item2;
            Origin = origin;
            SpriteOrigin = new Vector2(SpriteWidth, SpriteHeight);
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
        public void Pause() { State = AnimationState.Paused; }
        public void Update(GameTime gameTime)
        {
            if (State == AnimationState.Animated)
            {
                Time += gameTime.ElapsedGameTime.TotalSeconds;

                if (Time > TotalTime)
                {
                    Time = 0;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            var drawRect = new Rectangle(Origin.Item1,Origin.Item2 + CurrentYCoordinate * (SpriteHeight + Origin.Item2) - 1, SpriteWidth, SpriteHeight);
            spriteBatch.Draw(Texture, new Rectangle(position.ToPoint(), new Point(SpriteWidth * 3 , SpriteHeight * 3 )), drawRect, Color.White, 0, SpriteOrigin, spriteEffects, 0);
        }
    }
}
