using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Sprites
{
    public class AnimationFrame
    {
        public Sprite Sprite { get; set; } 
        public float Time { get; set; }
        public AnimationFrame(Sprite sprite, float time)
        {
            Sprite = sprite;
            Time = time;
        }
    }
}
