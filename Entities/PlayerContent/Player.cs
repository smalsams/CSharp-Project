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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

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
        public Vector2 Position;
        public Vector2 Velocity;
        public int Order { get; set; }
        public GameDirection Direction { get; private set; }
        public bool OnSolidObject { get; private set; } = true;
        public bool HasJumped { get; private set; } = true;
        public int Layer { get; private set; }
        public EventHandler Radio { get; set; }

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
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _stateProcessor.Draw(spriteBatch, Position, Direction == GameDirection.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }
        
        public void Update(GameTime gameTime)
        {
            if (State == PlayerState.Paused) return;
                _stateProcessor.Update(gameTime);
            Position += Velocity;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Velocity.X = Position.X > SPRITE_WIDTH*2 ? -4f : 0;
                Direction = GameDirection.Left;
                if (!HasJumped) Walk();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Velocity.X = 4f;
                Direction = GameDirection.Right;
                if(!HasJumped) Walk();
            }
            else
            {
                Velocity.X = 0;
            }

            if (!HasJumped && Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position.Y -= 10f;
                Velocity.Y = -5f;
                Jump();
                HasJumped = true;
                Radio.Invoke(this, EventArgs.Empty);
            }

            if (HasJumped)
            {
                Velocity.Y += 0.10f;
            }
            //temporary condition
            if (Position.Y + SPRITE_HEIGHT >= 800)
            {
                HasJumped = false;
            }

            if (HasJumped) return;
            Velocity.Y = 0;
            if (Velocity.X == 0)
            {
                Stop();
            }

        }
        public void Die() { IsAlive = false; }
        public void Walk()
        {
            _stateProcessor.ChangeCurrent(nameof(PlayerTextures.Walk));
            _stateProcessor.Current.Animate();
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

        public void Pause()
        {
            State = State == PlayerState.Paused ? PlayerState.Playing : PlayerState.Paused;
        }
    }
}
