using SamSer.Entities;
using SamSer.Entities.PlayerContent;
using System.Collections.Generic;

namespace SamSer.Control
{
    /// <summary>
    /// Capture of the Game State
    /// </summary>
    public class GameData
    {
        /// <remarks>
        /// Represents the level index for <see cref="Level"/> object in the JSON save object
        /// </remarks>
        public int Level { get; set; }
        /// <remarks>
        /// Represents the captured player data necessary for building the <see cref="Player"/> object from the JSON save object
        /// </remarks>
        public PlayerData PlayerData { get; set; }

        public List<EntityInfo> EntityData { get; set; }
    }
}
