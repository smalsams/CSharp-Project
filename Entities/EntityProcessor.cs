using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Entities
{
    public class EntityProcessor
    {
        public List<IEntity> entities = new List<IEntity>();
        public void Add(IEntity entity) { if (!entities.Contains(entity)) entities.Add(entity); return; }
        public void Remove(IEntity entity) {  entities.Remove(entity); return; }
        public void Clear() { entities.Clear(); return; }
        public IEntity this[int index] { get { return entities[index]; } }
        public void Draw()
        {

        }
    }
}
