using GameAttempt1.Control;
using GameAttempt1.Entities;
using GameAttempt1.Entities.PlayerContent;
using GameAttempt1.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameAttempt1
{
    public class TwoDPlatformer : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //sounds
        private Radio _radio;
        //entities
        private Player _player;
        private EntityProcessor _entityProcessor;
        private InputProcessor _inputProcessor;

        public TwoDPlatformer()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 960;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            var playerTextures = Content.Load<Texture2D>("Sprites/Tuxedo");
            _player = new Player(this, playerTextures)
            {
                Position = new Vector2(100, 100)
            };
            _inputProcessor = new InputProcessor(_player);


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            base.Update(gameTime);

            //update player
            _player.Update(gameTime);
            _inputProcessor.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);


            base.Draw(gameTime);
            _spriteBatch.Begin();
            _player.Draw(_spriteBatch,gameTime);
            _spriteBatch.End();
        }
    }
}