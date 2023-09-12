using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SamSer.Control
{
    /// <summary>
    /// Basic control of the screen using matrix transformations of the player's position
    /// </summary>
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public Vector2 Position = new(0, 0);
        public RectangleF CameraRectangle { get; set; }
        /// <summary>
        /// Makes the screen (camera) follow the <see cref="Player"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="graphics"></param>
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
            CameraRectangle = new RectangleF(Position.X, Position.Y, graphics.Viewport.Width, graphics.Viewport.Height);
            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }
    }
}
