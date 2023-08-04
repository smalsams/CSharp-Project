using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Microsoft.Xna.Framework;


namespace GameAttempt1.TileMap
{
    public class TileMapController
    {
        private TiledMap _tiledMap;
        private TiledMapRenderer _renderer;

        public void SetMap(TiledMap tiledMap)
        {
            _tiledMap = tiledMap;
        }
        public void Update(GameTime gameTime)
        {
            _renderer.Update(gameTime);
        }
    }
}
