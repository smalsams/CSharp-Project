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
using System.Linq;
using static SamSer.Utilities.GameUtilities;

namespace SamSer.Control
{
    /// <summary>
    /// Playable <see cref="State"/> instance. Acts as a game session, responsible for interaction between the environment and the player.
    /// </summary>
    public sealed class GameState : State
    {
        #region Fields
        private readonly Player _player;
        private readonly List<Component> _pauseComponents;
        private readonly LevelController _levelController;
        private SoundEffect _jumpSoundEffect;
        private Button _pauseButton;
        private Counter<int> _scoreCounter;
        private Counter<int> _healthCounter;
        private Texture2D _backgroundTexture;
        private Camera _camera;
        /// <remarks>
        /// Current score in the form of <see cref="int"/> representing the number of <see cref="Coin"/>s caught during the game
        /// </remarks>
        public int Score { get; set; }
        #endregion
        #region Constructors
        /// <summary>
        /// The Non-Json constructor for the <see cref="GameState"/>.
        /// </summary>
        /// <param name="contentManager">Object responsible for asset manipulation, for further documentation see <see cref="ContentManager"/></param>
        /// <param name="game">The game instance.</param>
        /// <param name="graphicsDevice"></param>
        /// <param name="levelController"></param>
        public GameState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice, LevelController levelController)
            : base(contentManager, game, graphicsDevice)
        {
            _pauseComponents = new List<Component>();
            var playerTextures = ContentManager.Load<Texture2D>("Sprites/Tuxedo");
            _player = new Player(playerTextures);
            _player.NoHealthEvent += ExitGame_OnClick;
            _levelController = levelController;
            _levelController.SetPlayerPosition += SetPlayerPosition_OnNextLevel;
            _levelController.Current.PlaySong();
            LoadComponents();
        }

        /// <summary>
        /// The Json constructor for the <see cref="GameState"/>.
        /// </summary>
        /// <param name="contentManager">Object responsible for asset manipulation, for further documentation see <see cref="ContentManager"/></param>
        /// <param name="game">The game instance.</param>
        /// <param name="graphicsDevice"></param>
        /// <param name="levelController"></param>
        /// <param name="playerData"></param>
        public GameState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice, LevelController levelController, PlayerData playerData)
            : base(contentManager, game, graphicsDevice)
        {
            _pauseComponents = new List<Component>();
            var playerTextureName = Player.GetTextureName();
            var playerTextures = ContentManager.Load<Texture2D>($"Sprites/{playerTextureName}");
            _player = new Player(playerData);
            _player.LoadTexture(playerTextures);
            _player.NoHealthEvent += ExitGame_OnClick;
            _levelController = levelController;
            _levelController.SetPlayerPosition += SetPlayerPosition_OnNextLevel;
            _levelController.Current.PlaySong();
            Score = playerData.Score;
            LoadComponents();
        }
        #endregion
        #region Content-Loading Methods
        /// <summary>
        /// Loads assets exclusive/non-exclusive for <see cref="GameState"/>
        /// into the content manager and creates components that these assets represent.
        /// </summary>
        public void LoadComponents()
        {
            _camera = new Camera();
            var pauseTexture = ContentManager.Load<Texture2D>("PauseButton");
            _pauseButton = new Button(pauseTexture, null, new Vector2(0, 0), "");
            _pauseButton.AddKeyboardInvoker(Keys.Escape);
            _pauseButton.ButtonPress += PauseGame_OnPress;

            _backgroundTexture = ContentManager.Load<Texture2D>("sky1");
            var buttonFont = ContentManager.Load<SpriteFont>("ButtonFont");
            var buttonTexture = ContentManager.Load<Texture2D>("Button");
            var sliderTexture = ContentManager.Load<Texture2D>("Slider");
            var sliderScrollTexture = ContentManager.Load<Texture2D>("SliderScroll");
            var counterFont = ContentManager.Load<SpriteFont>("VolSliderDescriptionFont");

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

            _scoreCounter = new Counter<int>(counterFont, new Vector2(1450, 0), "Score",Color.Beige, 0, () => Score);
            _healthCounter = new Counter<int>(counterFont, new Vector2(1450, 50), "Health", Color.Red, 3, () => _player.Health);

            _jumpSoundEffect = ContentManager.Load<SoundEffect>("Sounds/jumppp11");
            _player.JumpEvent += PlaySoundEffect_OnJump;
            _pauseComponents.Add(saveButton, mainMenuButton, volumeCounter, volumeSlider);
        }
        #endregion
        #region GameState Events
        /// <summary>
        /// Plays the jump sound effect.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event</param>
        /// <param name="e">Event data</param>
        public void PlaySoundEffect_OnJump(object sender, EventArgs e)
        {
            _jumpSoundEffect.Play();
        }

        /// <summary>
        /// Changes volume of the background music to the <see cref="float"/> number specified by UI logic.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event</param>
        /// <param name="volume"><see cref="float"/> number representing volume of the background music</param>
        private static void VolumeSlider_VolumeChanged(object sender, float volume)
        {
            MediaPlayer.Volume = volume;
        }
        /// <summary>
        /// Exits the game session and changes the <see cref="State"/> into <see cref="MainMenuState"/>.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event</param>
        /// <param name="e">Event data</param>
        private void ExitGame_OnClick(object sender, EventArgs e)
        {
            Game.ChangeState(new MainMenuState(ContentManager, Game, GraphicsDevice));
        }
        /// <summary>
        /// Saves the game state into a Json file according to the template provided by
        /// <see cref="PlayerData"/>, <see cref="EntityInfo"/> and
        /// <see cref="GameData"/>. The constructed file is then saved into one of four save slots.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event</param>
        /// <param name="e">Event data</param>
        private void SaveGame_OnClick(object sender, EventArgs e)
        {
            //reformats player data
            var data = new PlayerData
            {
                Position = _player.Position,
                State = _player.State,
                Direction = _player.Direction,
                Velocity = _player.Velocity,
                Score = Score
            };
            //reformats entity data
            var entityData = _levelController.Current.Entities.Select(entity => new EntityInfo
                {
                    Type = entity.GetType().Name,
                    Properties = JsonConvert.SerializeObject(entity, Formatting.Indented)
                })
                .ToList();
            //reformats level data
            var gameData = new GameData
            {
                PlayerData = data,
                Level = _levelController.Index,
                EntityData = entityData
            };
            //Serializes all formatted data into a json string
            var jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
            //saves the produced file into Saves folder into one of 4 slots
            var saveDirectory = Path.Combine(DIR_PATH_RELATIVE, "Saves");
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }
            var saveFiles = Directory.GetFiles(saveDirectory);
            var fileNum = 1;
            var filename = Path.Combine(saveDirectory, $"save{fileNum}");
            while (saveFiles.Contains(filename) && fileNum < 4)
            {
                fileNum++;
                filename = Path.Combine(saveDirectory, $"save{fileNum}");
            }
            if (!saveFiles.Contains(filename))
            {
                File.WriteAllText(filename, jsonData);
                return;
            }
            File.Delete(Path.Combine(saveDirectory, "save1"));
            for (var i = 2; i <= 4; i++)
            {
                File.Move(Path.Combine(saveDirectory, $"save{i}"), Path.Combine(saveDirectory, $"save{i - 1}"));
            }
            File.WriteAllText(filename, jsonData);

        }
        /// <summary>
        /// Pauses the level and all entities including the player
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event</param>
        /// <param name="e">Event data</param>
        private void PauseGame_OnPress(object sender, EventArgs e)
        {
            _levelController.Current.Pause();
            _player.Pause();
        }
        /// <summary>
        /// Sets the position of the player according to json constructed from the level data.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event</param>
        /// <param name="e">Event data</param>
        private void SetPlayerPosition_OnNextLevel(object sender, EventArgs e)
        {
            _player.PlayerDefaultX = _levelController.GetPlayerPosition.X;
            _player.PlayerDefaultY = _levelController.GetPlayerPosition.Y;
        }
        #endregion
        #region State Methods
        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        { 
            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: _camera.Transform);
            _player.Draw(spriteBatch, gameTime);
            _levelController.Draw(spriteBatch, gameTime, _camera.Transform);
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                _player.DrawDebug(GraphicsDevice);
                DrawDebug(GraphicsDevice, spriteBatch);
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
        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            PlayerExternalUpdate();
            _player.Update(gameTime);
            if (_player.Direction == GameDirection.Right
                && _player.Position.X > _levelController.Current.GlobalBounds.X - PLAYER_WIDTH * 2) _player.Velocity.X = 0;
            _camera.Follow(_player, GraphicsDevice);
            if (IsPlayerOutOfVerticalBounds())
            {
                _levelController.Current.ResetSong();
                _player.Die();
            }
            _pauseButton.Update(gameTime);
            _scoreCounter.Update(gameTime);
            _healthCounter.Update(gameTime);
            _levelController.Update(gameTime);
            _levelController.UpdateEntities(_player, _camera, out var scoreUpdate);
            Score += scoreUpdate;
            if (_player.State == PlayerState.Paused)
            {
                _pauseComponents.ForEach(c => c.Update(gameTime));
            }

        }
        #endregion
        #region Debug Methods
        /// <summary>
        /// Draws lines around all entities and platforms in the game, used for debug and test purposes
        /// </summary>
        /// <param name="gd"></param>
        /// <param name="spriteBatch"></param>
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
        /// <summary>
        /// Updates player according to collision or manipulation with external objects
        /// </summary>
        public void PlayerExternalUpdate()
        {
            var platforms = _levelController.Current.TiledMap.GetLayer<TiledMapObjectLayer>("Platforms")
                .Objects;
            var obstacles = _levelController.Current.TiledMap.GetLayer<TiledMapObjectLayer>("Obstacles").Objects;
            var finish = _levelController.Current.TiledMap.GetLayer<TiledMapObjectLayer>("LevelEnd").Objects;
            var waterTiles = _levelController.Current.TiledMap.GetLayer<TiledMapTileLayer>("WaterLayer").Tiles;
            var deadlyLayer = _levelController.Current.TiledMap.GetLayer<TiledMapTileLayer>("DeadlyLayer").Tiles;
            _player.ResetPhysicsValues();
            //Updates level if player collides with end of the level
            if ((from finishBlock in finish
                    select new RectangleF(finishBlock.Position.X, finishBlock.Position.Y, finishBlock.Size.Width,
                        finishBlock.Size.Height)
                    into rectangle
                    let playerRect = _player.BoundingBox
                    where rectangle.Intersects(playerRect)
                    select rectangle).Any())
            {
                _levelController.Current.LevelFinish.Invoke(_levelController.Current, EventArgs.Empty);
                _player.Position = new Vector2(100, 100);
                return;
            }
            //entity or player collision with platforms
            foreach (var platform in platforms)
            {
                var platformRectangle = new RectangleF(platform.Position.X, platform.Position.Y, platform.Size.Width, platform.Size.Height);
                _levelController.EntityTerrainCollision(platformRectangle);
                _player.HandleCollisionY(platformRectangle, true);
                _player.HandleCollisionX(platformRectangle);
            }
            // entity or player collision with obstacles
            foreach (var obstacle in obstacles)
            {
                var obstacleRectangle = new RectangleF(obstacle.Position.X, obstacle.Position.Y, obstacle.Size.Width, obstacle.Size.Height);
                _player.HandleCollisionY(obstacleRectangle, false);
                _player.HandleCollisionX(obstacleRectangle);
            }
            //player collision with water
            foreach (var waterTile in waterTiles)
            {
                var tileRectangle = new RectangleF(waterTile.X * 32, waterTile.Y * 32, 32, 32);
                if (!waterTile.IsBlank)
                {
                    _player.WaterCheck(tileRectangle);
                }
                
            }
            //player collision with deadly objects
            foreach(var deathObject in deadlyLayer)
            {
                var deathRectangle = new RectangleF(deathObject.X * 32, deathObject.Y * 32, 32, 32);
                if (deathObject.IsBlank) continue;
                if (_player.BoundingBox.Intersects(deathRectangle) && _player.InvulnerabilityTimer <= 0f)
                {
                    _player.Die();
                }
            }
        }
        /// <summary>
        /// Checks whether the player is lower than the vertical camera limit
        /// </summary>
        /// <returns></returns>
        public bool IsPlayerOutOfVerticalBounds()
        {
            return (_player.Position.Y > GraphicsDevice.Viewport.Height);
        }
        #endregion
    }
}
