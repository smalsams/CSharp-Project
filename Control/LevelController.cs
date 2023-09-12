using SamSer.Entities;
using SamSer.Entities.PlayerContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using static SamSer.Utilities.GameUtilities;

namespace SamSer.Control
{
    /// <summary>
    /// Main game control unit, 
    /// </summary>
    public sealed class LevelController
    {
        #region Fields and Properties


        private readonly List<string> _levelList;
        
        /// <remarks>Indicates level number</remarks>
        public int Index { get; set; }

        /// <remarks>Reference to the current level</remarks>
        public Level Current { get; private set; }
        private readonly GraphicsDevice _device;
        private readonly ContentManager _contentManager;
        public TextureManager TextureManager;
        private readonly SoundEffect _coinEffect;
        public Vector2 GetPlayerPosition { get; private set; }

        public EventHandler SetPlayerPosition;
        public EventHandler WinGame;


        #endregion
        #region Constructors


        public LevelController(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _levelList = new List<string>();
            LoadLevels(DIR_PATH_RELATIVE + "Content");
            _coinEffect = contentManager.Load<SoundEffect>(DIR_PATH_RELATIVE + "Content/Sounds/Picked Coin Echo");
            _contentManager = contentManager;
            _device = graphicsDevice;
            SetLevel(Index);
        }


        #endregion
        #region Load Methods


        /// <summary>
        /// Loads every name of the <see cref="Level"/> in the folder containing generated level i the form of .tmx file
        /// </summary>
        /// <param name="path">Path to the folder containing level names.</param>
        public void LoadLevels(string path)
        {
            const string suffix = LEVEL_FOLDER + "Level*.tmx";
            var fileNames = Directory.GetFiles(path, suffix, SearchOption.TopDirectoryOnly);
            _levelList.AddRange(fileNames.Select(Path.GetFileNameWithoutExtension));
        }


        /// <summary>
        /// Loads data from the memory for the specific level
        /// </summary>
        /// <param name="levelName">Name of the level whose data will be fetched</param>
        /// <returns></returns>
        private static LevelData LoadLevelData(string levelName)
        {
            var levelDataJson = File.ReadAllText($"{DIR_PATH_RELATIVE}LevelEntityData/{levelName}Data.json");
            return JsonConvert.DeserializeObject<LevelData>(levelDataJson, new Vector2JsonConverter());

        }


        #endregion
        #region Level Control

        /// <summary>
        /// Event that is invoked after finishing a level.
        /// </summary>
        /// <param name="sender">Reference to the object that raised the event</param>
        /// <param name="args"></param>
        public void NextLevel(object sender, EventArgs args)
        {
            //stops music
            MediaPlayer.Stop();
            //changes player position to new level's default position
            SetPlayerPosition.Invoke(this, EventArgs.Empty);
            //increases index
            Index++;
            if(Index >= _levelList.Count)
            {
                //win game if last level finish
                WinGame.Invoke(this, EventArgs.Empty);
                return;
            }
            //sets next level
            SetLevel(Index);
        }


        /// <summary>
        /// Sets a new level according to a <paramref name="levelIndex"/>
        /// </summary>
        /// <param name="levelIndex">An index of the new level to be set</param>
        public void SetLevel(int levelIndex)
        {
            Index = levelIndex;
            SetLevel(_levelList[levelIndex]);
        }


        /// <summary>
        /// Sets a new level according to the <paramref name="levelName"/>
        /// </summary>
        /// <param name="levelName">A name of the new level to be set</param>
        public void SetLevel(string levelName)
        {

            var tileMap = _contentManager.Load<TiledMap>(LEVEL_FOLDER + levelName);
            var renderer = new TiledMapRenderer(_device, tileMap);
            var song = _contentManager.Load<Song>($"Sounds/song_level{Index}");
            Current = new Level(tileMap, renderer, song);
            Current.LevelFinish += NextLevel;
            Current.PlaySong();
            var levelData = LoadLevelData(levelName);
            TextureManager = new TextureManager(_contentManager);
            foreach (var texture in levelData.Textures)
            {
                TextureManager.LoadTexture(texture);
            }
            foreach (var entityJson in levelData.Entities)
            {
                Current.Entities.Add(SetEntity(entityJson.Value));
            }
            GetPlayerPosition = levelData.PlayerPosition;
        }


        #endregion
        #region Entity Control


        /// <summary>
        /// Reformats data from the saved Json object and deserializes them into entity objects.
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns>A Json-Constructed <see cref="IEntity"/> object.</returns>
        /// <exception cref="JsonException">If the given entity type is not supported</exception>
        public IEntity SetEntity(JObject jsonObject)
        {
            var entityType = jsonObject["Class"]?.Value<string>();
            IEntity entity = entityType switch
            {
                nameof(Coin) => JsonConvert.DeserializeObject<Coin>(jsonObject.ToString(), new Vector2JsonConverter()),
                nameof(FlameEnemy) => JsonConvert.DeserializeObject<FlameEnemy>(jsonObject.ToString(), new Vector2JsonConverter()),
                _ => throw new JsonException($"Unknown entity type: {entityType}"),
            };
            var texture = entity.GetTextureName(jsonObject);
            entity.LoadTexture(TextureManager[texture]);
            return entity;
        }


        /// <summary>
        /// Checks collision of every entity with every terrain
        /// </summary>
        /// <param name="rectangle">Bounds of a terrain</param>
        public void EntityTerrainCollision(RectangleF rectangle)
        {
            foreach (var entity in Current.Entities.Where(entity => entity.GetType().IsAssignableTo(typeof(BaseEnemy))))
            {
                ((BaseEnemy)entity).CollisionX(rectangle);
                ((BaseEnemy)entity).CollisionY(rectangle);
            }
        }


        /// <summary>
        /// Updates every entity in the current level
        /// </summary>
        /// <param name="player">Reference to player</param>
        /// <param name="camera">Reference to the current game camera</param>
        /// <param name="newScoreAddition">New score after the update</param>
        public void UpdateEntities(Player player, Camera camera, out int newScoreAddition)
        {
            newScoreAddition = 0;
            for (var i = Current.Entities.Count - 1; i >= 0; i--)
            {
                var entity = Current.Entities[i];
                if (player.BoundingBox.Intersects(entity.BoundingBox))
                {
                    if (entity.GetType() == typeof(Coin))
                    {
                        newScoreAddition += (int)((Coin)entity).Type;
                        _coinEffect.Play();
                        Current.Entities.RemoveAt(i);
                    }
                    if (entity.GetType().IsAssignableTo(typeof(BaseEnemy)))
                    {
                        if (player.InvulnerabilityTimer <= 0f)
                        {
                            player.Die();
                        }
                    }
                }
                entity.InSight = camera.CameraRectangle.Intersects(entity.BoundingBox);
            }
        }


        /// <summary>
        /// Removes every entity from the entity list (from game) that is not within allowed bounds.
        /// </summary>
        public void RemoveOutOfBoundsEntities()
        {
            //current map bounds
            var mapBounds = new RectangleF(0, 0, Current.TiledMap.WidthInPixels, Current.TiledMap.HeightInPixels - UPPER_BOUND_OFFSET);

            //entity check
            for (var i = 0; i < Current.Entities.Count; i++)
            {
                var entity = Current.Entities[i];
                if (mapBounds.Contains(entity.Position)) continue;
                Current.Entities.RemoveAt(i);
                i--;
            }
        }


        #endregion
        #region Update/Draw Methods


        /// <summary>
        /// Draws the current level
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        /// <param name="viewMatrix"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Matrix viewMatrix)
        {
            Current.Draw(spriteBatch, gameTime, viewMatrix);
        }


        /// <summary>
        /// Updates object in the current level.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            Current.Update(gameTime);
            RemoveOutOfBoundsEntities();
        }
        #endregion
    }
}
