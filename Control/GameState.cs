using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using GameAttempt1.Entities;
using GameAttempt1.Entities.PlayerContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;

namespace GameAttempt1.Control
{
    public class GameState : State
    {
        private const int PLAYER_DEFAULT_X = 100;
        private const int PLAYER_DEFAULT_Y = 800;
        private readonly Player _player;
        private readonly InputProcessor _inputProcessor;
        private Texture2D _playerTextures;
        private Level _level;
        public GameState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice, Level level) 
            : base(contentManager, game, graphicsDevice)
        {
            _playerTextures = contentManager.Load<Texture2D>("Sprites/Tuxedo");
            _player = new Player(game, _playerTextures)
            {
                Position = new Vector2(PLAYER_DEFAULT_X, PLAYER_DEFAULT_Y)
            };
            _inputProcessor = new InputProcessor(_player);
            _level = level;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            _player.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            _inputProcessor.Update(gameTime);
        }
    }
}
