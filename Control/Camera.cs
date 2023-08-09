using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAttempt1.Control
{
    public class Camera
    {
        private Viewport _viewport;
        private Matrix _transform;
        private Vector2 _position;
        private float _rotation;
        private int _viewportWidth;
        private int _viewportHeight;
        private int _worldWidth;
        private int _worldHeight;
        public Camera(Viewport viewport, int worldWidth,
            int worldHeight)
        {
            _rotation = 0.0f;
            _position = Vector2.Zero;
            _viewportWidth = viewport.Width;
            _viewportHeight = viewport.Height;
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
        }
        public float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        public void Move(Vector2 amount)
        {
            _position += amount;
        }
        public Vector2 Pos
        {
            get => _position;
            set
            {
                var leftBarrier = (float)_viewportWidth *
                                  .5f;
                var rightBarrier = _worldWidth -
                                   (float)_viewportWidth * .5f;
                var topBarrier = _worldHeight -
                                 (float)_viewportHeight * .5f;
                var bottomBarrier = (float)_viewportHeight *
                                    .5f ;
                _position = value;
                if (_position.X < leftBarrier)
                    _position.X = leftBarrier;
                if (_position.X > rightBarrier)
                    _position.X = rightBarrier;
                if (_position.Y > topBarrier)
                    _position.Y = topBarrier;
                if (_position.Y < bottomBarrier)
                    _position.Y = bottomBarrier;
            }
        }
    }
}
