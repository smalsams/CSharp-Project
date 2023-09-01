using Microsoft.Xna.Framework;

namespace SamSer.Entities.PlayerContent
{
    public class PlayerStats
    {
        public int Id { get; set; }
        public Vector2 Position { get; set; }
        public Level Level { get; set; }
        public int Score { get; set; }
    }
}
