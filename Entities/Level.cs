using GameAttempt1.Entities.PlayerContent;
using GameAttempt1.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;

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
        public List<IEntity> Entities { get; set; }
        private bool _paused;
        public Vector2 GlobalBounds => new(TiledMap.WidthInPixels, TiledMap.HeightInPixels);
        public float TileSize => TiledMap.TileWidth;
        public bool Locked { get; set; }
        public Level(TiledMap map, TiledMapRenderer renderer, Song song)
        {
            TiledMap = map;
            _renderer = renderer;
            _song = song;
            Entities = new List<IEntity>();
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
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Matrix viewMatrix)
        {
            _renderer.Draw(viewMatrix);
            foreach (var entity in Entities)
            {
                entity.Draw(spriteBatch, gameTime);
            }
        }


        public void Update(GameTime gameTime)
        {
            _renderer.Update(gameTime);
            foreach (var entity in Entities)
            {
                entity.Update(gameTime);
            }
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
