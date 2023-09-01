﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SamSer.Components
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

        public EventHandler<float> SliderScroll;
        public Slider(Texture2D sliderTexture, Texture2D scrollTexture, Vector2 sliderPosition, float defaultValue = 1f)
        {
            _sliderTexture = sliderTexture;
            _scrollTexture = scrollTexture;
            SliderPosition = sliderPosition;
            _scrollPosition = new Vector2(SliderPosition.X + (_sliderTexture.Width * defaultValue) - _scrollTexture.Width, _sliderRectangle.Center.Y - _scrollRectangle.Height / 2);
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
                _scrollPosition.X = MathHelper.Clamp(mousePosition.X, _sliderRectangle.Left, _sliderRectangle.Right - _scrollRectangle.Width);
                _scrollPosition.Y = _sliderRectangle.Center.Y - _scrollRectangle.Height / 2;

                var percentage = (_scrollPosition.X - _sliderRectangle.Left) / (float)(_sliderRectangle.Width - _scrollRectangle.Width);
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
