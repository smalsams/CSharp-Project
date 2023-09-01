using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SamSer.Control
{
    public class LevelData
    {
        public Dictionary<string, JObject> Entities { get; set; }
        public List<string> Textures { get; set; }
        public JObject Player { get; set; }
    }
}
