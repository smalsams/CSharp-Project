using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Entities.PlayerContent
{
    public struct PlayerTextures
    {
        public Texture2D Walk { get; private set; }
        public Texture2D Sprint { get; private set; }
        public Texture2D None { get; private set; }
        public Texture2D Jump { get; private set; }
        public Texture2D Die { get; private set; }
        public Texture2D Duck { get; private set; }
        public Texture2D Swim { get; private set; }
    }
}
