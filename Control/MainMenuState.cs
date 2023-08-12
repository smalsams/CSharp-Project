#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameAttempt1.Components;
using GameAttempt1.Entities;
using GameAttempt1.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using GameAttempt1.Utilities;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

#endregion

namespace GameAttempt1.Control
{
    public class MainMenuState : State
    {
        #region Fields
        private const int MENU_X_COORDINATE = 650;
        private const int MENU_Y_COORDINATE = 320;
        private const int MENU_OFFSET = 100;
        private readonly List<Component> _components;
        private Texture2D _backgroundTexture;
        private Song song;
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        #endregion
        #region Constructors
        public MainMenuState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice)
            : base(contentManager, game, graphicsDevice)
        {
            _components = new List<Component>();
            LoadComponents();
        }
        #endregion
        #region State methods
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            spriteBatch.Begin();
            _components.ForEach(c => c.Draw(gameTime, spriteBatch));
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            _components.ForEach(c => c.Update(gameTime));
        }
        #endregion
        #region Content events
        public void NewGame_OnClick(object sender, EventArgs e)
        {
            StopMediaPlayer();
            _game.ChangeState(new GameState(_contentManager, _game, _graphicsDevice));
        }

        private static void VolumeSlider_VolumeChanged(object sender, float volume)
        {
            MediaPlayer.Volume = volume;
        }
        public void LoadGame_OnClick(object sender, EventArgs e)
        {
            StopMediaPlayer();
            Console.WriteLine("HAHAH");
        }

        public void ExitGame_OnClick(object sender, EventArgs e)
        {
            StopMediaPlayer();
            _game.Exit();
        }
        #endregion
        #region MediaPlayer control methods
        private void PlayMediaPlayer()
        {
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
        }

        private static void StopMediaPlayer()
        {
            MediaPlayer.Stop();
        }
        #endregion
        #region Content-Loading methods
        private void LoadComponents()
        {
            _backgroundTexture = _contentManager.Load<Texture2D>("country-platform-back");
            var titleFont = _contentManager.Load<SpriteFont>("TitleFont");
            var buttonFont = _contentManager.Load<SpriteFont>("ButtonFont");
            var volumeFont = _contentManager.Load<SpriteFont>("VolSliderDescriptionFont");
            var buttonTexture = _contentManager.Load<Texture2D>("Button");
            var sliderTexture = _contentManager.Load<Texture2D>("Slider");
            var sliderScrollTexture = _contentManager.Load<Texture2D>("SliderScroll");

            var gameTitle = new TextComponent(titleFont, new Vector2(550, 100), "GAME_TITLE");


            var startGameButton = new Button(buttonTexture,
                buttonFont,
                new Vector2(MENU_X_COORDINATE, MENU_Y_COORDINATE),
                "New Game");
            startGameButton.ButtonPress += NewGame_OnClick;

            var loadGameButton = new Button(buttonTexture,
                buttonFont,
                new Vector2(MENU_X_COORDINATE, MENU_Y_COORDINATE + MENU_OFFSET),
                "Load Game");
            loadGameButton.ButtonPress += LoadGame_OnClick;

            var exitButton = new Button(buttonTexture,
                buttonFont,
                new Vector2(MENU_X_COORDINATE, MENU_Y_COORDINATE + 2 * MENU_OFFSET),
                "Exit");
            exitButton.ButtonPress += ExitGame_OnClick;
            var volumeCounter = new Counter<float>(volumeFont, new Vector2(MENU_X_COORDINATE,
                MENU_Y_COORDINATE + 3 * MENU_OFFSET), "Volume", 100, () => (float)Math.Round(MediaPlayer.Volume * 100));
            var volumeSlider = new Slider(sliderTexture, sliderScrollTexture, new Vector2(MENU_X_COORDINATE,
                MENU_Y_COORDINATE + 4 * MENU_OFFSET));

            _components.Add(gameTitle, startGameButton, loadGameButton, exitButton, volumeSlider, volumeCounter);
            //sounds
            song = _contentManager.Load<Song>("Sounds/space");
            PlayMediaPlayer();
            volumeSlider.SliderScroll += VolumeSlider_VolumeChanged;
        }
        #endregion
    }
}