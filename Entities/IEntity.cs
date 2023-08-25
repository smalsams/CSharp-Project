using GameAttempt1.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json.Linq;
using System;
using System.Dynamic;
using System.Text.Json.Serialization;

namespace GameAttempt1.Entities
{
    [JsonConverter(typeof(EntityConverter))]
    public interface IEntity
    {
        int Id { get; set; }
        Vector2 Position { get; set; }
        RectangleF BoundingBox { get; }
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        void Update(GameTime gameTime);
        string GetTextureName(JObject jsonObject);
        void LoadTexture(Texture2D texture);
    }
}
