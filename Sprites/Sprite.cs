using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt1.Sprites
{
    public class Sprite
    {
        public Texture2D Texture { get; private set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public Vector2 Origin => new(Width/2, Height/2);
        public Sprite(Texture2D texture, int x, int y, int width, int height)
        {
            Texture = texture;
            XCoordinate = x;
            YCoordinate = y;
            Width = width;
            Height = height;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 coordinates, SpriteEffects spriteEffects) 
        {
            spriteBatch.Draw(Texture, coordinates, new Rectangle(XCoordinate, YCoordinate, Width, Height) , Color.White, 0,Origin, 0, spriteEffects, 0);
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
