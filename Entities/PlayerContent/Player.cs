using GameAttempt1.Control;
using GameAttempt1.Sounds;
using GameAttempt1.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GameAttempt1.Entities.PlayerContent
{
    [Serializable]
    public class Player : IEntity
    {
        public const int SPRITE_WIDTH = 20;
        public const int SPRITE_HEIGHT = 24;
        private SpriteStateProcessor _stateProcessor = new();
        public PlayerState State { get; private set; }
        public bool IsAlive { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; private set; } = new Vector2(4, 0);
        public int Order { get; set; }
        public int Direction { get; private set; }
        public bool OnSolidObject { get; private set; } = true;
        public int Layer { get; private set; }

        public Player(Game game, Texture2D playerTextures)
        {
            var width = playerTextures.Width;
            _stateProcessor.AddState(nameof(PlayerTextures.None), new Animation(playerTextures, 4, (6, 6), (SPRITE_WIDTH, SPRITE_HEIGHT)));
            _stateProcessor.AddState(nameof(PlayerTextures.Walk), new Animation(playerTextures, 6, (46, 6), (SPRITE_WIDTH, SPRITE_HEIGHT)));
            _stateProcessor.AddState(nameof(PlayerTextures.Jump), new Animation(playerTextures, 4, (86, 6), (SPRITE_WIDTH, SPRITE_HEIGHT)));
            _stateProcessor.AddState(nameof(PlayerTextures.Swim), new Animation(playerTextures, 6, (166, 6), (SPRITE_WIDTH, SPRITE_HEIGHT)));
            _stateProcessor.ChangeCurrent(nameof(PlayerTextures.None));
            _stateProcessor.Current?.Animate();
        }
        public void SetDirection(GameDirection direction)
        {
            Direction = direction switch
            {
                GameDirection.Left => -1,
                GameDirection.Right => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _stateProcessor.Draw(spriteBatch, Position, Direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }
        
        public void Update(GameTime gameTime)
        {
            _stateProcessor.Update(gameTime);
        }
        public void Die() { IsAlive = false; }
        public void Walk(GameDirection direction)
        {   
            SetDirection(direction);
            _stateProcessor.ChangeCurrent(nameof(PlayerTextures.Walk));
            _stateProcessor.Current.Animate();
            Position +=  Speed * Direction;
            //Position = Position.X < 0 ? new Vector2(810, Position.Y) : Position.X >= 810 ? new Vector2(10, Position.Y) : Position;
        }
        public void Stop() 
        {
            _stateProcessor.ChangeCurrent(nameof(PlayerTextures.None));
            _stateProcessor.Current.Animate();
        }
        public void Jump()
        {
            _stateProcessor.ChangeCurrent(nameof(PlayerTextures.Jump));
            _stateProcessor.Current.Animate();
        }
        public void Duck()
        {
            
        }


    }
}
