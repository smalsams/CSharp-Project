using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SamSer.Control
{
    /// <summary>
    /// Represents the state of the game object
    /// </summary>
    public abstract class State
    {
        protected ContentManager ContentManager;
        protected TwoDPlatformer Game;
        protected GraphicsDevice GraphicsDevice;
        /// <summary>
        /// Draws everything present on the screen in given state in real time
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Updates everything present on the screen in given state in real time
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        protected State(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice)
        {
            ContentManager = contentManager;
            Game = game;
            GraphicsDevice = graphicsDevice;
        }
    }
}
