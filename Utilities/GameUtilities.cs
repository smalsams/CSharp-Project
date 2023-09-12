using System;
using System.Collections.Generic;

namespace SamSer.Utilities
{
    /// <summary>
    /// Collection of useful constants of primitive types or methods.
    /// </summary>
    public static class GameUtilities
    {
        public const string DIR_PATH_RELATIVE = "../../../";
        public const int PLAYER_WIDTH = 20;
        public const int PLAYER_HEIGHT = 24;
        public const int MENU_X_COORDINATE = 650;
        public const int MENU_Y_COORDINATE = 320;
        public const int MENU_OFFSET = 100;
        public const float GRAVITY = 0.10f;
        public const float JUMP_HEIGHT = 10f;
        public const float DEFAULT_WALK_VELOCITY = 4f;
        public const float JUMP_Y_VELOCITY = 5f;
        public const int PLAYER_TEXTURE_X_OFFSET = 40;
        public const int PLAYER_TEXTURE_DEFAULT = 6;
        public const int PLAYER_WALK_ANI_COUNT = 4;
        public const int PLAYER_JUMP_ANI_COUNT = 6;
        public const int PLAYER_SWIM_ANI_COUNT = 4;
        public const int PLAYER_IDLE_ANI_COUNT = 6;
        public const int GAME_INCR_SCALE = 3;
        public const float WATER_RUSH_VELOCITY = 3f;
        public const float WATER_DEFAULT_VELOCITY = 1f;
        public const float COLLISION_THRESHOLD_Y = 10f;
        public const float COLLISION_THRESHOLD_X = 5f;
        public const float GRAVITY_LIMIT = 10f;
        public const int UPPER_BOUND_OFFSET = 60;
        public const int FPS = 10;
        public const string LEVEL_FOLDER = "./TilesetGen/";
        public const string PLAYER_TEXTURE_NAME = "Tuxedo";
        public static int Round(this float f) => (int)Math.Round(f);

        public static void Add<T>(this List<T> list, params T[] parameters)
        {
            list.AddRange(parameters);
        }
    }
}
