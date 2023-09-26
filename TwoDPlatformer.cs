using SamSer.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SamSer
{
    /// <summary>
    /// MonoGame class that handles the game loop and the drawing of the game, as well as the switching of states.
    /// See <see cref="State"/> for more information.
    /// Responsible for the creation of the game window and the graphics device.
    /// </summary>
    public sealed class TwoDPlatformer : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private State _currentState;
        private State _nextState;

        public TwoDPlatformer()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Changes the current state of the game to the specified state.
        /// </summary>
        /// <param name="state">State to be changed into</param>
        public void ChangeState(State state) => _nextState = state;
        /// <summary>
        /// Method that initializes the game window and the graphics device.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();
        }
        /// <summary>
        /// Loads the content of the game (passes the actual loading work to the responsible state instance).
        /// </summary>
        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState = new MainMenuState(Content, this, _graphics.GraphicsDevice);
        }
        /// <summary>
        /// Updates the game state (passes the actual updating work to the responsible state instance).
        /// </summary>
        /// <param name="gameTime"></param>
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
        /// <summary>
        /// Draws the game state (passes the actual drawing work to the responsible state instance).
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);
            base.Draw(gameTime);
            _currentState.Draw(gameTime, _spriteBatch);
        }
    }
}