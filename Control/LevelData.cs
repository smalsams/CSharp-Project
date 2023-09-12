using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SamSer.Control
{
    /// <summary>
    /// Capture of the level state
    /// </summary>
    public class LevelData
    {
        public Dictionary<string, JObject> Entities { get; set; }
        public List<string> Textures { get; set; }
        public Vector2 PlayerPosition { get; set; }
    }
}
