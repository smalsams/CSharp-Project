using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameAttempt1.TileMap
{
    public class TileMap
    {
        private TiledMap _tiledMap;
        private TiledMapRenderer _renderer;

        public TileMap(string path, ContentManager content, GraphicsDevice graphics)
        {
            _tiledMap = content.Load<TiledMap>(path);
            _renderer = new TiledMapRenderer(graphics, _tiledMap);
        }
    }
}
