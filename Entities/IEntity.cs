using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace SamSer.Entities
{
    /// <summary>
    /// Defines a base non-playable object
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Additional identification of the entity in the form of an Id
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Top-Left point of the entity on the game screen as a vector
        /// </summary>
        Vector2 Position { get; set; }
        bool Paused { get; set; }
        bool InSight { get; set; }
        RectangleF BoundingBox { get; }
        /// <summary>
        /// Draws necessary textures on screen in real time
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        /// <summary>
        /// Updates every action and texture on screen in real time
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Gets texture name from a partially de-serialized JSON object belonging to an entity
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        string GetTextureName(JObject jsonObject);
        /// <summary>
        /// Loads texture for the entity
        /// </summary>
        /// <param name="texture"></param>
        void LoadTexture(Texture2D texture);
    }
}