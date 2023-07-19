using GameAttempt1.Entities.PlayerContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Control
{
    public class InputProcessor
    {
        private Player _player;
        private KeyboardState _previousKey;

        public InputProcessor(Player player) { _player = player; }
        public void Update(GameTime gameTime)
        {
            Process(gameTime);
        }
        private void Process(GameTime gameTime) 
        {
            var KState = Keyboard.GetState();
            if (ShouldWalkLeft(KState)) { _player.Walk(GameDirection.Left); }
            else if(ShouldWalkRight(KState)) { _player.Walk(GameDirection.Right); }
            else { _player.Stop(); }
            if(ShouldJump(KState)){
                _player.Jump();
            }
            _previousKey = KState;
        }
        private bool ShouldWalkLeft(KeyboardState currentState)
        {
            return currentState.IsKeyDown(Keys.A) && _player.OnSolidObject;
        }

        private bool ShouldWalkRight(KeyboardState currentState)
        {
            return currentState.IsKeyDown(Keys.D) && _player.OnSolidObject;
        }

        private bool ShouldJump(KeyboardState currentState)
        {
            return (currentState.IsKeyDown(Keys.W) || currentState.IsKeyDown(Keys.Space))
                && !(_previousKey.IsKeyDown(Keys.Space) || _previousKey.IsKeyDown(Keys.W));
        }
    }
}
