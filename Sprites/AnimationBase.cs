using SamSer.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SamSer.Sprites
{
    public abstract class AnimationBase
    {
        protected static int FPS => GameUtilities.FPS;
        public Texture2D Texture { get; }
        public int TextureLength { get; }
        public int Scale { get; }
        public Point Origin { get; }
        public int SpriteWidth { get; }
        public int SpriteHeight { get; }
        public Vector2 MapOrigin => new Vector2(SpriteWidth / 2, SpriteHeight / 2);
        public AnimationState State { get; protected set; } = AnimationState.Cancelled;
        public double Time { get; protected set; }
        public double TotalTime => (double)TextureLength / FPS;
        protected AnimationBase(Texture2D texture, int spriteAnimationCount, Point textureOrigin, Size spriteDimensions, int entityScale = 1)
        {
            Texture = texture;
            TextureLength = spriteAnimationCount;
            Origin = textureOrigin;
            SpriteWidth = spriteDimensions.Width;
            SpriteHeight = spriteDimensions.Height;
            Scale = entityScale;
            Time = 0;
        }
        public void Animate() => State = AnimationState.Animated;
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects);
    }
}
