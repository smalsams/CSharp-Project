using System.Collections.Generic;

namespace GameAttempt1.Entities
{
    public class EntityProcessor
    {
        public List<IEntity> entities = new();
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

        public void Draw()
        {

        }
    }
}
