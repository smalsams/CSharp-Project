using GameAttempt1.Entities.EntityBehaviour;
using GameAttempt1.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameAttempt1.Entities.PlayerContent
{
    [Serializable]
    public class Player : IEntity
    {
        private SpriteStateProcessor _stateProcessor = new();
        public PlayerState State { get; private set; }
        public bool IsAlive { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Speed { get; private set; }
        public int Order { get; set; }
        public int Direction { get; private set; }
        public Player(PlayerTextures playerTextures)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        public void Die() { IsAlive = false; }
        public void Move()
        {

            Speed = new Vector2(1, 0) * Speed * Direction;
        }
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
