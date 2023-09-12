using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SamSer.Components
{
    /// <summary>
    /// Base for any reserved or user defined area on screen
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// Draws the component area onto the game screen
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        /// <summary>
        /// Updates the component area every tick.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);
    }
}
