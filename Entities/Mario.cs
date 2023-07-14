using GameAttempt1.Entities.EntityBehaviour;
using GameAttempt1.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameAttempt1.Entities
{
    [Serializable]
    public class Mario : IEntity
    {
        public Sprite Sprite { get; private set; }
        public MarioState State { get; private set; }
        public bool IsAlive { get; private set; }
        public Vector2 Position { get; private set; }
        public float Speed { get; private set; }  
        public int Order { get; set; }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        public void Die() { }
        public void Jump()
        {
            throw new NotImplementedException();
        }
        public void Sprint()
        {
            throw new NotImplementedException();
        }
        public void Duck()
        {
            throw new NotImplementedException();
        }
    }
}
