using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;

namespace GameAttempt1.Components
{
    public sealed class Slider : Component
    {
        private readonly Texture2D _sliderTexture;
        private readonly Texture2D _scrollTexture;
        private Rectangle _sliderRectangle => new((int)SliderPosition.X, (int)SliderPosition.Y, _sliderTexture.Width, _sliderTexture.Height);

        private Rectangle _scrollRectangle => new((int)_scrollPosition.X, (int)_scrollPosition.Y, _scrollTexture.Width,
            _scrollTexture.Height);
        private bool _isSliderClicked;
        public Vector2 SliderPosition { get; set; }
        private Vector2 _scrollPosition;

        public EventHandler<float> VolumeChanged;
        public Slider(Texture2D sliderTexture,Texture2D scrollTexture, Vector2 sliderPosition)
        {
            _sliderTexture = sliderTexture;
            _scrollTexture = scrollTexture;
            SliderPosition = sliderPosition;
            _scrollPosition = new Vector2(SliderPosition.X + _scrollTexture.Width / 2, SliderPosition.Y);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sliderTexture, _sliderRectangle, Color.SkyBlue);
            spriteBatch.Draw(_scrollTexture, _scrollRectangle, Color.SkyBlue);
        }

        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed && _sliderRectangle.Contains(mousePosition))
            {
                _isSliderClicked = true;
            }
            if (_isSliderClicked)
            {
                _scrollPosition.X = MathHelper.Clamp(mousePosition.X - _sliderRectangle.Width / 2, _sliderRectangle.Left, _sliderRectangle.Right - _scrollRectangle.Width);
                _scrollPosition.Y = _sliderRectangle.Center.Y - _scrollRectangle.Height / 2;

                var volumePercentage = (_scrollPosition.X - _sliderRectangle.Left) / (float)(_sliderRectangle.Width - _scrollRectangle.Width);
                volumePercentage = MathHelper.Clamp(volumePercentage, 0f, 1f);
                VolumeChanged?.Invoke(this, volumePercentage);
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                _isSliderClicked = false;
            }
        }
    }
}
