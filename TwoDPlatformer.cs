using System.Collections.Generic;
using GameAttempt1.Control;
using GameAttempt1.Entities;
using GameAttempt1.Entities.PlayerContent;
using GameAttempt1.Sounds;
using GameAttempt1.TileMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System.IO;

namespace GameAttempt1
{
    public class TwoDPlatformer : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TileMapController _tileMapController;
        private State _currentState;
        private State _nextState;

        public TwoDPlatformer()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        public void ChangeState(State state) => _nextState = state;
        protected override void Initialize()
        {

            base.Initialize();
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState = new MainMenuState(Content, this, _graphics.GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (_nextState is not null)
            {
                _currentState = _nextState;
                _nextState = null;
            }
            _currentState.Update(gameTime); 
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);
            base.Draw(gameTime);
            _currentState.Draw(gameTime, _spriteBatch);
        }
    }
}