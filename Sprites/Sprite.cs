using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Sprites
{
    public class Sprite
    {
        public Texture2D Texture { get; private set; }
        public int X_Coordinate { get; set; }
        public int Y_Coordinate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Sprite(Texture2D texture, int x, int y, int width, int height)
        {
            Texture = texture;
            X_Coordinate = x;
            Y_Coordinate = y;
            Width = width;
            Height = height;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 coordinates) { }
    }
}
