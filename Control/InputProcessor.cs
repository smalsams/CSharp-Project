using GameAttempt1.Entities.PlayerContent;
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
        private Player Player { get; set; }
        
        public InputProcessor(Player player) { Player = player; }
        public void Process(GameTime gameTime) { }
    }
}
