using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace GameAttempt1.Utilities
{
    public static class GameUtilities
    {
        public const float MAX = int.MaxValue;
        public const float MIN = int.MinValue;
        public const float ZERO = 0.0001f;
        public const int PLAYER_WIDTH = 20;
        public const int PLAYER_HEIGHT = 24;
        public const int MENU_X_COORDINATE = 650;
        public const int MENU_Y_COORDINATE = 320;
        public const int MENU_OFFSET = 100;
        public const int PLAYER_DEFAULT_X = 100;
        public const int PLAYER_DEFAULT_Y = 700;
        public const float GRAVITY = 0.15f;
        public const float JUMP_HEIGHT = 10f;
        public const float DEFAULT_WALK_VELOCITY = 4f;
        public const float JUMP_Y_VELOCITY = 5f;
        public const int PLAYER_TEXTURE_X_OFFSET = 40;
        public const int PLAYER_TEXTURE_DEFAULT = 6;
        public static int Round(this float f) => (int)Math.Round(f);

        public static void Add<T>(this List<T> list, params T[] parameters)
        {
            list.AddRange(parameters);
        }
    }
}
