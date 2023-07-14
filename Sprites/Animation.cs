using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Sprites
{
    public class Animation
    {
        private List<AnimationFrame> frames = new();
        public bool IsAnimated { get; private set; }
        public void Update(GameTime gameTime) { }
        public void AddFrame(Sprite sprite, float time) { 
            frames.Add(new AnimationFrame(sprite, time));
        }
        public void Cancel() => IsAnimated = false;
        public void Start() => IsAnimated = true;
        public AnimationFrame this[int index]
        {
            get { return index < frames.Count && index >= 0 ? frames[index] : throw new ArgumentOutOfRangeException($"There is no frame at index {index}."); }
        }
    }
}
