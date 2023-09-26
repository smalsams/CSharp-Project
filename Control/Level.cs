using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using SamSer.Entities;
using System;
using System.Collections.Generic;

namespace SamSer.Control
{
    /// <summary>
    /// Implementation of the single map in the game
    /// </summary>
    public class Level
    {
        private readonly Song _song;
        public TiledMap TiledMap;
        private readonly TiledMapRenderer _renderer;
        public EventHandler LevelFinish;
        /// <remarks>List of every <see cref="Level"/> specific <see cref="IEntity"/></remarks>
        public List<IEntity> Entities { get; set; }
        private bool _paused;
        /// <remarks>Size of the level</remarks>
        public Vector2 GlobalBounds => new(TiledMap.WidthInPixels, TiledMap.HeightInPixels);

        public Level(TiledMap map, TiledMapRenderer renderer, Song song)
        {
            TiledMap = map;
            _renderer = renderer;
            _song = song;
            Entities = new List<IEntity>();
        }
        /// <summary>
        /// Controls the action in the <see cref="Level"/> if the level is paused/unpaused
        /// </summary>
        public void Pause()
        {
            _paused = !_paused;
            if (_paused)
            {
                MediaPlayer.Pause();
                foreach (var entity in Entities)
                {
                    entity.Paused = true;
                }
            }
            else
            {
                MediaPlayer.Resume();
                foreach (var entity in Entities)
                {
                    entity.Paused = false;
                }
            }
        }
        /// <summary>
        /// Draws the entities specific to the single <see cref="Level"/> onto the screen while applying matrix transformation and creating a camera focus effect
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        /// <param name="viewMatrix"></param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Matrix viewMatrix)
        {
            _renderer.Draw(viewMatrix);
            foreach (var entity in Entities)
            {
                entity.Draw(spriteBatch, gameTime);
            }
        }

        /// <summary>
        /// Updates objects characteristic to the <see cref="Level"/> every tick
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _renderer.Update(gameTime);
            foreach (var entity in Entities)
            {
                entity.Update(gameTime);
            }
        }
        /// <summary>
        /// Plays the <see cref="Level"/> specific theme.
        /// </summary>
        public void PlaySong()
        {
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 1f;
        }
        /// <summary>
        /// Makes the song play from the beginning again.
        /// </summary>
        public void ResetSong()
        {
            MediaPlayer.Stop();
            PlaySong();
        }
    }
}
