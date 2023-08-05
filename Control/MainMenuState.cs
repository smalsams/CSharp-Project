using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAttempt1.Components;
using GameAttempt1.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GameAttempt1.Control
{
    public class MainMenuState : State
    {
        private const int MENU_X_COORDINATE = 650;
        private const int MENU_Y_COORDINATE = 320;
        private const int MENU_OFFSET = 100;
        private List<Component> _components;
        private Song song;
        public MainMenuState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice) 
            : base(contentManager, game, graphicsDevice)
        {
            var buttonFont = _contentManager.Load<SpriteFont>("ButtonFont");
            var buttonTexture = _contentManager.Load<Texture2D>("Button");
            var startGameButton = new Button(buttonTexture,
                buttonFont,
                new Vector2(MENU_X_COORDINATE, MENU_Y_COORDINATE),
                "Start Game");
            startGameButton.ButtonPress += StartGameClick;
            
            var loadGameButton = new Button(buttonTexture,
                buttonFont,
                new Vector2(MENU_X_COORDINATE, MENU_Y_COORDINATE + MENU_OFFSET),
                "Load Game");
            loadGameButton.ButtonPress += LoadGameClick;

            var exitButton = new Button(buttonTexture,
                buttonFont,
                new Vector2(MENU_X_COORDINATE, MENU_Y_COORDINATE + 2*MENU_OFFSET),
                "Exit");
            exitButton.ButtonPress += ExitGameClick;
            _components = new List<Component>()
            {
                startGameButton,
                loadGameButton,
                exitButton
            };

            //sounds
            var mainMenuTheme = _contentManager.Load<Song>("Sounds/space");
            MediaPlayer.Play(mainMenuTheme);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
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
        void MediaPlayer_MediaStateChanged(object sender, System.
            EventArgs e)
        {
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(song);
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
