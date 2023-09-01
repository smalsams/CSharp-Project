using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SamSer.Control
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position = new(0, 0);
        public void Follow(IFocusable target, GraphicsDevice graphics)
        {
            if (target.Position.X >= graphics.Viewport.Width / 2)
            {
                Position.X = target.Position.X - graphics.Viewport.Width / 2;
            }
            else
            {
                Position.X = 0;
            }
            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }
    }
}
