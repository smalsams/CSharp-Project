using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SamSer.Sprites
{
    public class SpriteStateProcessor
    {
        private Dictionary<string, AnimationBase> _automaton = new();
        public AnimationBase Current { get; private set; }
        public void AddState(string name, AnimationBase state)
        {
            _automaton.Add(name, state);
        }
        public void AddState(AnimationBase state)
        {
            _automaton.Add(nameof(state), state);
        }
        public void ChangeCurrent(string name) => Current = _automaton[name];
        public void RemoveState(string name)
        {
            var state = _automaton[name];
            if (state is null) return;
            RemoveState(state);
        }
        public void RemoveState(AnimationBase state)
        {
            _automaton.Remove(nameof(state));
            if (Current == state)
            {
                Current = null;
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            Current?.Draw(spriteBatch, position, spriteEffects);
        }
        public void Update(GameTime gameTime)
        {
            Current?.Update(gameTime);
        }
    }
}
