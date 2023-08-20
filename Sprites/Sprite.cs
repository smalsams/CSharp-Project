using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAttempt1.Sprites
{
    public class Sprite
    {
        public Texture2D Texture { get; private set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private float _rotation;

        public Vector2 Origin => new(Width / 2, Height / 2);
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
            spriteBatch.Draw(Texture, coordinates, new Rectangle(XCoordinate, YCoordinate, Width, Height), Color.White, _rotation, Origin, 0, spriteEffects, 0);
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
