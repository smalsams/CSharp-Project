using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAttempt1.Entities
{
    public interface IEntity
    {
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
