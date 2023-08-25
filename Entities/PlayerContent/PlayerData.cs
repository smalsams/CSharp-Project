using GameAttempt1.Control;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Entities.PlayerContent
{
    public class PlayerData
    {
        [Required]
        public Vector2 Position { get; set; }
        public PlayerState State { get;  set; }
        public GameDirection Direction { get; set; }
        public Vector2 Velocity { get; set; }
    }
}
