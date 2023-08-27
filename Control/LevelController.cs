using GameAttempt1.Entities;
using GameAttempt1.Entities.PlayerContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Tiled.Renderers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static GameAttempt1.Utilities.GameUtilities;


namespace GameAttempt1.Control
{
    public sealed class LevelController
    {
        private readonly List<string> _levelList;
        public int Index { get; set; }
        private readonly GraphicsDevice _device;
        private readonly ContentManager _contentManager;
        private TextureManager _textureManager;
        private PlayerData _playerData;

        public LevelController(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _levelList = new List<string>();
            LoadLevels(DIR_PATH_RELATIVE + "Content/");
            _contentManager = contentManager;
            _device = graphicsDevice;
            SetLevel(Index);
            Current.LevelFinish += NextLevel;
        }

        public void LoadLevels(string path)
        {
            const string suffix = "TilesetGen/Level*.tmx";
            var filenames = Directory.GetFiles(path, suffix, SearchOption.TopDirectoryOnly);
            _levelList.AddRange(filenames.Select(Path.GetFileNameWithoutExtension));
        }
        private LevelData LoadLevelData(string levelName)
        {
            var levelDataJson = File.ReadAllText($"{DIR_PATH_RELATIVE}LevelEntityData/{levelName}Data.json");
            return JsonConvert.DeserializeObject<LevelData>(levelDataJson);

        }
        public void NextLevel(object sender, EventArgs args)
        {
            MediaPlayer.Stop();
            Index++;
            Index %= _levelList.Count;
            SetLevel(Index);
        }

        public void SetLevel(string levelName)
        {
            var tileMap = _contentManager.Load<TiledMap>("./TilesetGen/" + levelName);
            var renderer = new TiledMapRenderer(_device, tileMap);
            var song = _contentManager.Load<Song>($"Sounds/song_level{Index}");
            Current = new Level(tileMap, renderer, song);
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
            if(levelData.Player is not null)
            {
                _playerData = JsonConvert.DeserializeObject<PlayerData>(levelData.Player.ToString());
            }
        }
        public IEntity SetEntity(JObject jsonObject)
        {
            var entityType = jsonObject["Class"]?.Value<string>();

            IEntity entity;
            switch (entityType)
            {
                case "Coin":
                    entity = JsonConvert.DeserializeObject<Coin>(jsonObject.ToString(), new Vector2JsonConverter());
                    break;
                default:
                    throw new JsonException($"Unknown entity type: {entityType}");
            }
            var texture = entity.GetTextureName(jsonObject);
            entity.LoadTexture(_textureManager[texture]);
            return entity;
        }
        public PlayerData GetPlayerData() => _playerData;

        public void SetLevel(int levelIndex)
        {
            SetLevel(_levelList[levelIndex]);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Matrix viewMatrix)
        {
            Current.Draw(spriteBatch, gameTime, viewMatrix);
        }
        public void Update(GameTime gameTime)
        {
            Current.Update(gameTime);
        }
        public Level Current { get; private set; }

        public bool PlayerEntityCollision(Player player, IEntity entity)
        {
            return player.BoundingBox.Intersects(entity.BoundingBox);
        }
    }
}
