using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAttempt1.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameAttempt1.Control
{
    public abstract class State
    {
        protected ContentManager _contentManager;
        protected TwoDPlatformer _game;
        protected GraphicsDevice _graphicsDevice;
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

        protected State(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice)
        {
            _contentManager = contentManager;
            _game = game;
            _graphicsDevice = graphicsDevice;
        }
    }
}
