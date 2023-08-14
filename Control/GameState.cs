using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using GameAttempt1.Components;
using GameAttempt1.Entities;
using GameAttempt1.Entities.PlayerContent;
using GameAttempt1.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Tiled;

namespace GameAttempt1.Control
{
    public class GameState : State
    {
        
        private readonly Player _player;
        private List<Component> _pauseComponents;
        private Texture2D _playerTextures;
        private LevelController _levelController;
        private SoundEffect _jumpSoundEffect;
        private Button _pauseButton;
        public GameState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice) 
            : base(contentManager, game, graphicsDevice)
        {
            _pauseComponents = new List<Component>();
            _playerTextures = _contentManager.Load<Texture2D>("Sprites/Tuxedo");
            _player = new Player(_game, _playerTextures)
            {
                Position = new Vector2(GameUtilities.PLAYER_DEFAULT_X, GameUtilities.PLAYER_DEFAULT_Y),
            };
            _levelController = new LevelController(contentManager, graphicsDevice);
            _player.LevelController = _levelController;
            LoadComponents();
        }

        public void LoadComponents()
        {
            var pauseTexture = _contentManager.Load<Texture2D>("PauseButton");
            _pauseButton = new Button(pauseTexture, null, new Vector2(0, 0), "");
            _pauseButton.AddKeyboardInvoker(Keys.Escape);
            _pauseButton.ButtonPress += PauseGame_OnPress;

            var buttonFont = _contentManager.Load<SpriteFont>("ButtonFont");
            var buttonTexture = _contentManager.Load<Texture2D>("Button");
            var saveButton = new Button(buttonTexture, buttonFont,
                new Vector2(GameUtilities.MENU_X_COORDINATE, GameUtilities.MENU_Y_COORDINATE),
                "Save");
            saveButton.ButtonPress += SaveGame_OnClick;
            var mainMenuButton = new Button(buttonTexture, buttonFont,
                new Vector2(GameUtilities.MENU_X_COORDINATE, GameUtilities.MENU_Y_COORDINATE + GameUtilities.MENU_OFFSET),
                "Exit");
            mainMenuButton.ButtonPress += ExitGame_OnClick;
            _jumpSoundEffect = _contentManager.Load<SoundEffect>("Sounds/jumppp11");
            _player.Radio += PlaySoundEffect_OnJump;
            _pauseComponents.Add(saveButton, mainMenuButton);
        }
        public void PlaySoundEffect_OnJump(object sender, EventArgs e)
        {
            _jumpSoundEffect.Play();
        }
        public void ExitGame_OnClick(object sender, EventArgs e)
        {
            _game.ChangeState(new MainMenuState(_contentManager, _game, _graphicsDevice));
        }

        public void SaveGame_OnClick(object sender, EventArgs e)
        {

        }
        public void PauseGame_OnPress(object sender, EventArgs e)
        {
            _levelController.Current.Pause();
            _player.Pause();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            _player.Draw(spriteBatch, gameTime);
            _pauseButton.Draw(gameTime, spriteBatch);
            _levelController.Draw(gameTime);
            if (_player.State == PlayerState.Paused)
            {
                _pauseComponents.ForEach(c => c.Draw(gameTime, spriteBatch));
            }
            spriteBatch.End();
        }
        public bool CheckPlatformY()
        {
            return _levelController.Current.TiledMap.GetLayer<TiledMapObjectLayer>("Platforms")
                .Objects.Any(platform => _player.Position.Y >= platform.Position.Y &&
                                         _player.Position.X > platform.Position.X &&
                                         _player.Position.X - 2 * GameUtilities.PLAYER_WIDTH < platform.Position.X + platform.Size.Width &&
                                         _player.Position.Y < platform.Position.Y + platform.Size.Height);
        }
        public bool CheckPlatformX()
        {
            return _levelController.Current.TiledMap.GetLayer<TiledMapObjectLayer>("Platforms")
                .Objects.Any(platform => _player.Position.X + GameUtilities.PLAYER_WIDTH >= platform.Position.X &&
                                         _player.Position.Y - GameUtilities.PLAYER_HEIGHT > platform.Position.Y &&
                                         _player.Position.Y < platform.Position.Y + platform.Size.Height &&
                                         _player.Position.X - 2*GameUtilities.PLAYER_WIDTH <= platform.Position.X + platform.Size.Width);
        }

        private void PlayerExternalUpdate()
        {
            _player.Colliding = CheckPlatformX();
            _player.OnPlatform = CheckPlatformY();
        }
        public override void Update(GameTime gameTime)
        {
            PlayerExternalUpdate();
            _player.Update(gameTime);
            _pauseButton.Update(gameTime);
            _levelController.Update(gameTime);
            if( _player.State == PlayerState.Paused){
                _pauseComponents.ForEach(c => c.Update(gameTime));
            }

        }
    }
}
