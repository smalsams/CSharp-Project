using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SamSer.Components
{
    /// <summary>
    /// Slider component, which allows for an area on screen to get mapped onto a range of possible values according to the position on the screen.
    /// Allows for user defined event to be invoked every time the position of the slider is changed.
    /// </summary>
    public sealed class Slider : Component
    {

        private readonly Texture2D _sliderTexture;
        private readonly Texture2D _scrollTexture;

        private Rectangle SliderRectangle => new((int)SliderPosition.X, (int)SliderPosition.Y, _sliderTexture.Width,
            _sliderTexture.Height);
        private Rectangle ScrollRectangle => new((int)_scrollPosition.X, (int)_scrollPosition.Y, _scrollTexture.Width,
            _scrollTexture.Height);
        private bool _isSliderClicked;
        /// <remarks>
        /// Position of the slider on the screen, which acts as the ranged area for the scroll
        /// </remarks>
        public Vector2 SliderPosition { get; set; }
        /// <remarks>
        /// Position of the scroll, which has a direct mapping onto the value in a specific range
        /// </remarks>
        private Vector2 _scrollPosition;
        /// <summary>
        /// User defined event, which is invoked every time the position of the slider is changed
        /// </summary>
        public EventHandler<float> SliderScroll;
        public Slider(Texture2D sliderTexture, Texture2D scrollTexture, Vector2 sliderPosition, float defaultValue = 1f)
        {
            _sliderTexture = sliderTexture;
            _scrollTexture = scrollTexture;
            SliderPosition = sliderPosition;
            _scrollPosition =
                new Vector2(SliderPosition.X + (_sliderTexture.Width * defaultValue) - _scrollTexture.Width,
                    SliderRectangle.Center.Y - ScrollRectangle.Height / 2);
        }
        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sliderTexture, SliderRectangle, Color.SkyBlue);
            spriteBatch.Draw(_scrollTexture, ScrollRectangle, Color.SkyBlue);
        }
        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed && SliderRectangle.Contains(mousePosition))
            {
                _isSliderClicked = true;
            }
            if (_isSliderClicked)
            {
                //maps the position of the scroll onto the range of values
                _scrollPosition.X = MathHelper.Clamp(mousePosition.X, SliderRectangle.Left,
                    SliderRectangle.Right - ScrollRectangle.Width);
                _scrollPosition.Y = SliderRectangle.Center.Y - ScrollRectangle.Height / 2;

                var percentage = (_scrollPosition.X - SliderRectangle.Left) / (SliderRectangle.Width - ScrollRectangle.Width);
                percentage = MathHelper.Clamp(percentage, 0f, 1f);
                SliderScroll?.Invoke(this, percentage);
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                _isSliderClicked = false;
            }
        }
    }
}
