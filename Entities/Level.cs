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
        private Song _song;
        public TiledMap TiledMap;
        private TiledMapRenderer _renderer;
        public EventHandler LevelFinish;
        public EntityController Controller;
        private bool _paused;
        public Vector2 GlobalBounds => new(TiledMap.WidthInPixels, TiledMap.HeightInPixels);
        public float TileSize => TiledMap.TileWidth;
        public bool Locked { get; set; }
        public Level(TiledMap map, TiledMapRenderer renderer, Song song)
        {
            TiledMap = map;
            _renderer = renderer;
            _song = song;
        }

        public void Pause()
        {
            _paused = !_paused;
            if (_paused)
            {
                MediaPlayer.Pause();
            }
            else
            {
                MediaPlayer.Resume();
            }
        }
        public void Draw(GameTime gameTime, Matrix viewMatrix)
        {
            _renderer.Draw(viewMatrix);

        }

        public void Update(GameTime gameTime)
        {
            if (_paused) return;
            _renderer.Update(gameTime);
        }
        public void PlaySong()
        {
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 1f;
        }
        public void ResetSong()
        {
            MediaPlayer.Stop();
            PlaySong();
        }
    }
}
