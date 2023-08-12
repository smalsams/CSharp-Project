using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAttempt1.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace GameAttempt1.Entities
{
    public class Level
    {
        private Song _song;
        private Radio _radio;
        private TiledMap _tiledMap;
        private TiledMapRenderer _renderer;
        public EventHandler LevelFinish;
        public Level(TiledMap map, TiledMapRenderer renderer)
        {
            _tiledMap = map;
            _renderer = renderer;
        }

        public void Pause()
        {
        }

        public void Draw(GameTime gameTime)
        {
            _renderer.Draw();
        }

        public void Update(GameTime gameTime)
        {
            _renderer.Update(gameTime);
        }
    }
}
