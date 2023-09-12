using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SamSer.Sprites
{
    /// <summary>
    /// Data structure storing the animation states and their assigned names
    /// </summary>
    public class SpriteStateProcessor
    {
        
        private readonly Dictionary<string, AnimationBase> _automaton = new();
        /// <remarks>The current animation state</remarks>
        public AnimationBase Current { get; private set; }
        /// <summary>
        /// Adds a state with an original name to the sprite processor
        /// </summary>
        /// <param name="name">Name of the new state</param>
        /// <param name="state">The new state</param>
        public void AddState(string name, AnimationBase state)
        {
            _automaton.Add(name, state);
        }
        /// <summary>
        /// Changes the current state to a different one via a <see cref="string"/> name
        /// </summary>
        /// <param name="name">Name of the new state</param>
        public void ChangeCurrent(string name) => Current = _automaton[name];

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
