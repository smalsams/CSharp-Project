using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Utilities
{
    public static class GameUtilities
    {
        public const float MAX = int.MaxValue;
        public const float MIN = int.MinValue;
        public const float ZERO = 0.0001f;
        public static int Round(this float f) => (int)Math.Round(f);

        public static void Add<T>(this List<T> list, params T[] parameters)
        {
            list.AddRange(parameters);
        }
    }
}
