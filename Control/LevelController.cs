using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAttempt1.Entities;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace GameAttempt1.Control
{
    public class LevelController
    {
        private List<string> _levelList;
        private int _index;
        private GraphicsDevice _device;
        private ContentManager _contentManager;

        public LevelController(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _levelList = new List<string>();
            _contentManager = contentManager;
            _device = graphicsDevice;
            SetLevel("TilesetGen/Test");
            Current.LevelFinish += NextLevel;
        }

        public void NextLevel(object sender, EventArgs args)
        {
            _index++;
            _index %= _levelList.Count;
            SetLevel(_levelList[_index]);
        }

        public void SetLevel(string levelName)
        {
            var tileMap = _contentManager.Load<TiledMap>(levelName);
            var renderer = new TiledMapRenderer(_device, tileMap);
            Current = new Level(tileMap, renderer);
        }

        public void Draw(GameTime gameTime)
        {
            Current.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            Current.Update(gameTime);
        }
        public Level Current { get; private set; }
    }
}
