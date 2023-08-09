using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using GameAttempt1.Components;
using GameAttempt1.Entities;
using GameAttempt1.Entities.PlayerContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;

namespace GameAttempt1.Control
{
    public class GameState : State
    {
        private const int PLAYER_DEFAULT_X = 100;
        private const int PLAYER_DEFAULT_Y = 800;
        private readonly Player _player;
        private List<Component> _pauseComponents;
        private Texture2D _playerTextures;
        private Level _level;
        private Button _pauseButton;
        public GameState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice, Level level) 
            : base(contentManager, game, graphicsDevice)
        {
            _playerTextures = contentManager.Load<Texture2D>("Sprites/Tuxedo");
            _player = new Player(game, _playerTextures)
            {
                Position = new Vector2(PLAYER_DEFAULT_X, PLAYER_DEFAULT_Y)
            };
            _level = level;
            var pauseTexture = contentManager.Load<Texture2D>("PauseButton");
            _pauseButton = new Button(pauseTexture, null, new Vector2(0, 0), "");
            _pauseButton.AddKeyboardInvoker(Keys.Escape);
            _pauseButton.ButtonPress += PauseGame_OnPress;
            _pauseComponents = new List<Component>()
            {
                
            };
        }

        public void PauseGame_OnPress(object sender, EventArgs e)
        {
            _level.Pause();
            _player.Pause();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            _player.Draw(spriteBatch, gameTime);
            _pauseButton.Draw(gameTime, spriteBatch);
            if (_player.State == PlayerState.Paused)
            {
                _pauseComponents.ForEach(c => c.Draw(gameTime, spriteBatch));
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            _pauseButton.Update(gameTime);
            if( _player.State == PlayerState.Paused ){
                _pauseComponents.ForEach(c => c.Update(gameTime));
            }
        }
    }
}
