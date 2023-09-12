using SamSer.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SamSer.Sprites
{
    /// <summary>
    /// Base implementation of an animation consisting of basic texture switching
    /// </summary>
    public abstract class AnimationBase
    {
        /// <remarks>Number of textures that get appear within a second</remarks>
        protected static int Fps => GameUtilities.FPS;
        /// <remarks>Texture containing the sprite sheet</remarks>
        public Texture2D Texture { get; }
        /// <remarks>Length of the sprite sheet</remarks>
        public int TextureLength { get; }
        /// <remarks>Defines how much will the actual size differ from the default one</remarks>
        public int Scale { get; }
        /// <remarks>Defines where the sprite center is on the sprite sheet</remarks>
        public Point Origin { get; }
        /// <remarks>Width of the single sprite</remarks>
        public int SpriteWidth { get; }
        /// <remarks>Height of the single sprite</remarks>
        public int SpriteHeight { get; }
        /// <remarks>Defines where the actual sprite center will be</remarks>
        public Vector2 MapOrigin => new(SpriteWidth / 2, SpriteHeight / 2);
        /// <remarks>Determines the state of the animation</remarks>
        public AnimationState State { get; protected set; } = AnimationState.Cancelled;
        /// <remarks>The current time</remarks>
        public double Time { get; protected set; }
        /// <remarks>Total possible time for the whole animation</remarks>
        public double TotalTime => (double)TextureLength / Fps;
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
        /// <summary>
        /// Starts the current animation
        /// </summary>
        public void Animate() => State = AnimationState.Animated;
        /// <summary>
        /// Updates the animation every frame
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);
        /// <summary>
        /// Draws the current texture with a given <see cref="SpriteEffect"/>
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="spriteEffects"></param>
        public abstract void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects);
    }
}
