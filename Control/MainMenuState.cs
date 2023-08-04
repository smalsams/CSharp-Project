using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAttempt1.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameAttempt1.Control
{
    public class MainMenuState : State
    {
        private List<Component> _components;
        public MainMenuState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice) 
            : base(contentManager, game, graphicsDevice)
        {
            var buttonFont = _contentManager.Load<SpriteFont>("ButtonFont");
            var buttonTexture = _contentManager.Load<Texture2D>("Button");
            var startGameButton = new Button(buttonTexture,
                buttonFont,
                new Vector2(650, 320),
                "Start Game");
            startGameButton.ButtonPress += StartGameClick;
            
            var loadGameButton = new Button(buttonTexture,
                buttonFont,
                new Vector2(650, 420),
                "Load Game");
            loadGameButton.ButtonPress += LoadGameClick;

            var exitButton = new Button(buttonTexture,
                buttonFont,
                new Vector2(650, 520),
                "Exit");
            exitButton.ButtonPress += ExitGameClick;
            _components = new List<Component>()
            {
                startGameButton,
                loadGameButton,
                exitButton
            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var item in _components)
            {
                item.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            _components.ForEach(c => c.Update(gameTime));
        }

        public void StartGameClick(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_contentManager, _game, _graphicsDevice));
        }
        public void LoadGameClick(object sender, EventArgs e)
        {
            Console.WriteLine("HAHAH");

        }
        public void ExitGameClick(object  sender, EventArgs e) { _game.Exit(); }
    }
}
