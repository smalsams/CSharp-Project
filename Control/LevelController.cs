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
using static SamSer.Utilities.GameUtilities;

namespace SamSer.Control
{
    public sealed class LevelController
    {
        #region Fields and Properties
        private readonly List<string> _levelList;
        public int Index { get; set; }
        public Level Current { get; private set; }
        private readonly GraphicsDevice _device;
        private readonly ContentManager _contentManager;
        private TextureManager _textureManager;
        private PlayerData _playerData;
        public PlayerData GetPlayerData => _playerData;
        #endregion
        #region Constructors

        public LevelController(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _levelList = new List<string>();
            LoadLevels(DIR_PATH_RELATIVE + "Content/");
            _contentManager = contentManager;
            _device = graphicsDevice;
            SetLevel(Index);
        }
        #endregion
        #region Load Methods
        public void LoadLevels(string path)
        {
            const string suffix = "TilesetGen/Level*.tmx";
            var filenames = Directory.GetFiles(path, suffix, SearchOption.TopDirectoryOnly);
            _levelList.AddRange(filenames.Select(Path.GetFileNameWithoutExtension));
        }
        private static LevelData LoadLevelData(string levelName)
        {
            var levelDataJson = File.ReadAllText($"{DIR_PATH_RELATIVE}LevelEntityData/{levelName}Data.json");
            return JsonConvert.DeserializeObject<LevelData>(levelDataJson);

        }
        #endregion
        #region Level Control
        public void NextLevel(object sender, EventArgs args)
        {
            MediaPlayer.Stop();
            Index++;
            Index %= _levelList.Count;
            SetLevel(Index);
        }
        public void SetLevel(int levelIndex)
        {
            SetLevel(_levelList[levelIndex]);
        }


        public void SetLevel(string levelName)
        {
            var tileMap = _contentManager.Load<TiledMap>("./TilesetGen/" + levelName);
            var renderer = new TiledMapRenderer(_device, tileMap);
            var song = _contentManager.Load<Song>($"Sounds/song_level{Index}");
            Current = new Level(tileMap, renderer, song);
            Current.LevelFinish += NextLevel;
            var levelData = LoadLevelData(levelName);
            _textureManager = new TextureManager(_contentManager);
            foreach (var texture in levelData.Textures)
            {
                _textureManager.LoadTexture(texture);
            }
            foreach (var entityJson in levelData.Entities)
            {
                Current.Entities.Add(SetEntity(entityJson.Value));
            }
            if (levelData.Player is not null)
            {
                _playerData = JsonConvert.DeserializeObject<PlayerData>(levelData.Player.ToString());
            }
        }
        #endregion
        #region Entity Control
        public IEntity SetEntity(JObject jsonObject)
        {
            var entityType = jsonObject["Class"]?.Value<string>();
            IEntity entity = entityType switch
            {
                "Coin" => JsonConvert.DeserializeObject<Coin>(jsonObject.ToString(), new Vector2JsonConverter()),
                "FlameEnemy" => JsonConvert.DeserializeObject<FlameEnemy>(jsonObject.ToString(), new Vector2JsonConverter()),
                _ => throw new JsonException($"Unknown entity type: {entityType}"),
            };
            var texture = entity.GetTextureName(jsonObject);
            entity.LoadTexture(_textureManager[texture]);
            return entity;
        }
        public void EntityTerrainCollision(RectangleF rectangle)
        {
            foreach (var entity in Current.Entities)
            {
                if (entity.GetType().IsAssignableTo(typeof(BaseEnemy)))
                {
                    ((BaseEnemy)entity).CollisionX(rectangle);
                    ((BaseEnemy)entity).CollisionY(rectangle);
                }
            }
        }
        public static bool PlayerEntityCollision(Player player, IEntity entity)
        {

            return player.BoundingBox.Intersects(entity.BoundingBox);
        }
        #endregion
        #region Update/Draw Methods

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Matrix viewMatrix)
        {
            Current.Draw(spriteBatch, gameTime, viewMatrix);
        }
        public void Update(GameTime gameTime)
        {
            Current.Update(gameTime);
        }
        #endregion
    }
}
