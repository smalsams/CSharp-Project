using SamSer.Components;
using SamSer.Entities;
using SamSer.Entities.PlayerContent;
using SamSer.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static SamSer.Utilities.GameUtilities;

namespace SamSer.Control
{
    public sealed class GameState : State
    {
        #region Fields
        private readonly Player _player;
        private readonly List<Component> _pauseComponents;
        private readonly Texture2D _playerTextures;
        private readonly LevelController _levelController;
        private SoundEffect _jumpSoundEffect;
        private Button _pauseButton;
        private Counter<int> _scoreCounter;
        private Counter<int> _healthCounter;
        private Camera _camera;
        public int Score { get; set; }
        #endregion
        #region Constructors
        public GameState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice, LevelController levelController)
            : base(contentManager, game, graphicsDevice)
        {
            _pauseComponents = new List<Component>();
            _playerTextures = _contentManager.Load<Texture2D>("Sprites/Tuxedo");
            _player = new Player(_playerTextures)
            {
                Position = new Vector2(PLAYER_DEFAULT_X, PLAYER_DEFAULT_Y),
            };
            _player.NoHealthEvent += ExitGame_OnClick;
            _levelController = levelController;
            _levelController.Current.PlaySong();
            LoadComponents();
        }
        public GameState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice, LevelController levelController, PlayerData playerData)
            : base(contentManager, game, graphicsDevice)
        {
            _pauseComponents = new List<Component>();
            var playerTextureName = Player.GetTextureName();
            _playerTextures = _contentManager.Load<Texture2D>($"Sprites/{playerTextureName}");
            _player = new Player(playerData);
            _player.LoadTexture(_playerTextures);
            _player.NoHealthEvent += ExitGame_OnClick;
            _levelController = levelController;
            _levelController.Current.PlaySong();
            Score = playerData.Score;
            LoadComponents();
        }
        #endregion
        #region Content-Loading Methods
        public void LoadComponents()
        {
            _camera = new Camera();
            var pauseTexture = _contentManager.Load<Texture2D>("PauseButton");
            _pauseButton = new Button(pauseTexture, null, new Vector2(0, 0), "");
            _pauseButton.AddKeyboardInvoker(Keys.Escape);
            _pauseButton.ButtonPress += PauseGame_OnPress;

            var buttonFont = _contentManager.Load<SpriteFont>("ButtonFont");
            var buttonTexture = _contentManager.Load<Texture2D>("Button");
            var sliderTexture = _contentManager.Load<Texture2D>("Slider");
            var sliderScrollTexture = _contentManager.Load<Texture2D>("SliderScroll");
            var counterFont = _contentManager.Load<SpriteFont>("VolSliderDescriptionFont");

            var saveButton = new Button(buttonTexture, buttonFont,
                new Vector2(MENU_X_COORDINATE, MENU_Y_COORDINATE),
                "Save");
            saveButton.ButtonPress += SaveGame_OnClick;
            var mainMenuButton = new Button(buttonTexture, buttonFont,
                new Vector2(MENU_X_COORDINATE, MENU_Y_COORDINATE + MENU_OFFSET),
                "Exit");
            mainMenuButton.ButtonPress += ExitGame_OnClick;

            var volumeCounter = new Counter<float>(counterFont, new Vector2(MENU_X_COORDINATE,
            MENU_Y_COORDINATE + 3 * MENU_OFFSET), "Volume", 100, () => (float)Math.Round(MediaPlayer.Volume * 100));
            var volumeSlider = new Slider(sliderTexture, sliderScrollTexture, new Vector2(MENU_X_COORDINATE,
                MENU_Y_COORDINATE + 4 * MENU_OFFSET));
            volumeSlider.SliderScroll += VolumeSlider_VolumeChanged;

            _scoreCounter = new Counter<int>(counterFont, new Vector2(1500, 0), "Score",Color.Beige, 0, () => Score);
            _healthCounter = new Counter<int>(counterFont, new Vector2(1500, 50), "Health", Color.Red, 3, () => _player.Health);

            _jumpSoundEffect = _contentManager.Load<SoundEffect>("Sounds/jumppp11");
            _player.Radio += PlaySoundEffect_OnJump;
            _pauseComponents.Add(saveButton, mainMenuButton, volumeCounter, volumeSlider);
        }
        #endregion
        #region GameState Events
        public void PlaySoundEffect_OnJump(object sender, EventArgs e)
        {
            _jumpSoundEffect.Play();
        }
        private static void VolumeSlider_VolumeChanged(object sender, float volume)
        {
            MediaPlayer.Volume = volume;
        }
        public void ExitGame_OnClick(object sender, EventArgs e)
        {
            _game.ChangeState(new MainMenuState(_contentManager, _game, _graphicsDevice));
        }

        public void SaveGame_OnClick(object sender, EventArgs e)
        {
            var data = new PlayerData
            {
                Position = _player.Position,
                State = _player.State,
                Direction = _player.Direction,
                Velocity = _player.Velocity,
                Score = Score
            };
            var gameData = new GameData
            {
                PlayerData = data,
                Level = _levelController.Index,
            };

            string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            int fileNum = 1;
            var filename = $"{DIR_PATH_RELATIVE}Saves/save{fileNum}";
            while (File.Exists(filename) && fileNum < 4)
            {
                fileNum++;
                filename = $"{DIR_PATH_RELATIVE}Saves/save{fileNum}";
            }
            if (!File.Exists(filename))
            {
                File.WriteAllText(filename, jsonData);
                return;
            }
            File.Delete($"{DIR_PATH_RELATIVE}Saves/save1");
            File.Move($"{DIR_PATH_RELATIVE}Saves/save2", $"{DIR_PATH_RELATIVE}Saves/save1");
            File.Move($"{DIR_PATH_RELATIVE}Saves/save3", $"{DIR_PATH_RELATIVE}Saves/save2");
            File.Move($"{DIR_PATH_RELATIVE}Saves/save4", $"{DIR_PATH_RELATIVE}Saves/save3");
            File.WriteAllText(filename, jsonData);

        }
        public void PauseGame_OnPress(object sender, EventArgs e)
        {
            _levelController.Current.Pause();
            _player.Pause();
        }
        #endregion
        #region State Methods
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: _camera.Transform);
            _player.Draw(spriteBatch, gameTime);
            _levelController.Draw(spriteBatch, gameTime, _camera.Transform);
            _player.DrawDebug(_graphicsDevice);
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                DrawDebug(_graphicsDevice, spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin();
            _pauseButton.Draw(gameTime, spriteBatch);
            _scoreCounter.Draw(gameTime, spriteBatch);
            _healthCounter.Draw(gameTime, spriteBatch);
            if (_player.State == PlayerState.Paused)
            {
                _pauseComponents.ForEach(c => c.Draw(gameTime, spriteBatch));
            }
            spriteBatch.End();
        }
        public override void Update(GameTime gameTime)
        {
            PlayerExternalUpdate();
            _player.Update(gameTime);
            if (_player.Direction == GameDirection.Right
                && _player.Position.X > _levelController.Current.GlobalBounds.X - PLAYER_WIDTH * 2) _player.Velocity.X = 0;
            _camera.Follow(_player, _graphicsDevice);
            if (IsPlayerDead())
            {
                _levelController.Current.ResetSong();
                _player.Reset();
            }
            _pauseButton.Update(gameTime);
            _scoreCounter.Update(gameTime);
            _healthCounter.Update(gameTime);
            _levelController.Update(gameTime);
            foreach (var entity in _levelController.Current.Entities)
            {
                if (LevelController.PlayerEntityCollision(_player, entity))
                {
                    if (entity.GetType() == typeof(Coin))
                    {
                        if (!((Coin)entity).Picked)
                        {
                            Score+=((int)((Coin)entity).Type);
                        }
                        ((Coin)entity).Picked = true;
                    }
                    if (entity.GetType().IsAssignableTo(typeof(BaseEnemy)))
                    {
                        if (_player.invulnerabilityTimer <= 0f)
                        {
                            _player.Reset();
                        }
                    }
                }
            }
            if (_player.State == PlayerState.Paused)
            {
                _pauseComponents.ForEach(c => c.Update(gameTime));
            }

        }
        #endregion
        #region Debug Methods
        public void DrawDebug(GraphicsDevice gd, SpriteBatch spriteBatch)
        {
            var textures = _levelController.Current.TiledMap.GetLayer<TiledMapObjectLayer>("Platforms").Objects;
            foreach (var t in textures)
            {
                var rectangleTexture2D = new Texture2D(gd, (int)t.Size.Width, (int)t.Size.Height);
                var colours = new List<Color>();
                for (var i = 0; i < (int)t.Size.Width; i++)
                {
                    for (var j = 0; j < (int)t.Size.Height; j++)
                    {
                        if (i == 0 || j == 0 || i == (int)t.Size.Width - 1 || j == (int)t.Size.Height - 1)
                        {
                            colours.Add(new Color(255, 255, 255, 255));
                        }
                        else
                        {
                            colours.Add(new Color(0, 0, 0, 0));
                        }
                    }
                }

                rectangleTexture2D.SetData(colours.ToArray());
                spriteBatch.Draw(rectangleTexture2D, t.Position, Color.Red);
            }
            foreach (var e in _levelController.Current.Entities)
            {
                var rectangleTexture2D = new Texture2D(gd, (int)e.BoundingBox.Width, (int)e.BoundingBox.Height);
                var colours = new List<Color>();
                for (var i = 0; i < (int)e.BoundingBox.Width; i++)
                {
                    for (var j = 0; j < (int)e.BoundingBox.Height; j++)
                    {
                        if (i == 0 || j == 0 || i == (int)e.BoundingBox.Width - 1 || j == (int)e.BoundingBox.Height - 1)
                        {
                            colours.Add(new Color(255, 255, 255, 255));
                        }
                        else
                        {
                            colours.Add(new Color(0, 0, 0, 0));
                        }
                    }
                }

                rectangleTexture2D.SetData(colours.ToArray());
                spriteBatch.Draw(rectangleTexture2D, e.Position, Color.Red);
            }
        }
        #endregion
        #region External Player Related Methods

        public void PlayerExternalUpdate()
        {
            var platforms = _levelController.Current.TiledMap.GetLayer<TiledMapObjectLayer>("Platforms")
                .Objects;
            var obstacles = _levelController.Current.TiledMap.GetLayer<TiledMapObjectLayer>("Obstacles").Objects;
            var finish = _levelController.Current.TiledMap.GetLayer<TiledMapObjectLayer>("LevelEnd").Objects;
            var waterTiles = _levelController.Current.TiledMap.GetLayer<TiledMapTileLayer>("WaterLayer").Tiles;
            _player.CollidingFromTop = false;
            _player.CollidingFromBottom = false;
            _player.CollidingFromLeft = false;
            _player.CollidingFromRight = false;
            _player.InWater = false;

            foreach (var finishBlock in finish)
            {
                var rectangle = new RectangleF(finishBlock.Position.X, finishBlock.Position.Y, finishBlock.Size.Width, finishBlock.Size.Height);
                var playerRect = _player.BoundingBox;
                if (rectangle.Intersects(playerRect))
                {
                    _levelController.Current.LevelFinish.Invoke(_levelController.Current, EventArgs.Empty);
                    _player.Position = new Vector2(100, 100);
                    return;
                }
            }

            foreach (var platform in platforms)
            {
                var platformRectangle = new RectangleF(platform.Position.X, platform.Position.Y, platform.Size.Width, platform.Size.Height);
                _levelController.EntityTerrainCollision(platformRectangle);
                _player.HandleCollisionY(platformRectangle, true);
                _player.HandleCollisionX(platformRectangle);
            }
            foreach (var obstacle in obstacles)
            {
                var obstacleRectangle = new RectangleF(obstacle.Position.X, obstacle.Position.Y, obstacle.Size.Width, obstacle.Size.Height);
                _player.HandleCollisionY(obstacleRectangle, false);
                _player.HandleCollisionX(obstacleRectangle);
            }
            foreach (var waterTile in waterTiles)
            {
                var tileRectangle = new RectangleF(waterTile.X * 32, waterTile.Y * 32, 32, 32);
                if (!waterTile.IsBlank)
                {
                    _player.WaterCheck(tileRectangle);
                }
            }
        }

        public bool IsPlayerDead()
        {
            return (_player.Position.Y > _graphicsDevice.Viewport.Height);
        }
        #endregion
    }
}
