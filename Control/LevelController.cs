using GameAttempt1.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace GameAttempt1.Control
{
    public sealed class LevelController
    {
        private List<string> _levelList;
        private int _index;
        private GraphicsDevice _device;
        private ContentManager _contentManager;

        public LevelController(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _levelList = new List<string>();
            LoadLevels("../../../Content/");
            _contentManager = contentManager;
            _device = graphicsDevice;
            SetLevel(_index);
            Current.LevelFinish += NextLevel;
        }

        public void LoadLevels(string path)
        {
            const string suffix = "TilesetGen/Level*.tmx";
            var filenames = Directory.GetFiles(path, suffix, SearchOption.TopDirectoryOnly);
            _levelList.AddRange(filenames.Select(Path.GetFileNameWithoutExtension));
        }
        public void NextLevel(object sender, EventArgs args)
        {
            _index++;
            _index %= _levelList.Count;
            SetLevel(_index);
        }

        public void SetLevel(string levelName)
        {
            var tileMap = _contentManager.Load<TiledMap>("./TilesetGen/" + levelName);
            var renderer = new TiledMapRenderer(_device, tileMap);
            Current = new Level(tileMap, renderer);
        }

        public void SetLevel(int levelIndex)
        {
            SetLevel(_levelList[levelIndex]);
        }

        public void Draw(GameTime gameTime, Matrix viewMatrix)
        {
            Current.Draw(gameTime, viewMatrix);
        }

        public void Update(GameTime gameTime)
        {
            Current.Update(gameTime);
        }
        public Level Current { get; private set; }
    }
}
