using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameAttempt1.Entities
{
    public class EntityController
    {
        public List<IEntity> entities = new();
        public List<IEntity> activeEntities = new();
        public void Add(IEntity entity)
        {
            if (!entities.Contains(entity)) entities.Add(entity);
        }
        public void Remove(IEntity entity)
        {
            entities.Remove(entity);
        }
        public void Clear()
        {
            entities.Clear();
        }
        public IEntity this[int index] => entities[index];

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var entity in activeEntities)
            {
                entity.Draw(spriteBatch, gameTime);
            }
        }
    }
}
