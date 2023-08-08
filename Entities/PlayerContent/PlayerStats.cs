using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Microsoft.Xna.Framework;

namespace GameAttempt1.Entities.PlayerContent
{
    public class PlayerStats
    {
        public int Id { get; set; }
        public Vector2 Position { get; set; }
        public Level Level { get; set; }
        public int Score { get; set; }
    }
}
