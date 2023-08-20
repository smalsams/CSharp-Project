using GameAttempt1.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;

namespace GameAttempt1.Entities
{
    public class Level
    {
        private Radio _radio;
        public TiledMap TiledMap;
        private TiledMapRenderer _renderer;
        public EventHandler LevelFinish;
        public Vector2 GlobalBounds => new(TiledMap.WidthInPixels, TiledMap.HeightInPixels);
        public float TileSize => TiledMap.TileWidth;
        public bool Locked { get; set; }
        public Level(TiledMap map, TiledMapRenderer renderer)
        {
            TiledMap = map;
            _renderer = renderer;
        }

        public void Pause()
        {
        }

        public void Draw(GameTime gameTime, Matrix viewMatrix)
        {
            _renderer.Draw(viewMatrix);

        }

        public void Update(GameTime gameTime)
        {
            _renderer.Update(gameTime);
        }
    }
}
