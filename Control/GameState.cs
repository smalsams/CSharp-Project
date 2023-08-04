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
        private readonly Player _player;
        private readonly InputProcessor _inputProcessor;
        private Texture2D _playerTextures;
        private EntityProcessor _entityProcessor;
        private Level _level;
        public GameState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice) : base(contentManager, game, graphicsDevice)
        {
            _playerTextures = contentManager.Load<Texture2D>("Sprites/Tuxedo");
            _player = new Player(game, _playerTextures)
            {
                Position = new Vector2(100, 100)
            };
            _inputProcessor = new InputProcessor(_player);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
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
