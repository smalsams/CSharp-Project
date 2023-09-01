using SamSer.Control;
using Microsoft.Xna.Framework;
using System.ComponentModel.DataAnnotations;

namespace SamSer.Entities.PlayerContent
{
    public class PlayerData
    {
        [Required]
        public Vector2 Position { get; set; }
        public PlayerState State { get; set; }
        public GameDirection Direction { get; set; }
        public Vector2 Velocity { get; set; }
        public int Score { get; set; }
    }
}
