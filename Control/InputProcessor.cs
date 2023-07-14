using GameAttempt1.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Control
{
    public class InputProcessor
    {
        private Mario Mario { get; set; }
        
        public InputProcessor(Mario mario) { Mario = mario; }
        public void Process(GameTime gameTime) { }
    }
}
