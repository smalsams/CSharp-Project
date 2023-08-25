using GameAttempt1.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json.Serialization;

namespace GameAttempt1.Entities
{
    [JsonConverter(typeof(EntityConverter))]
    public class FlameEnemy : BaseEnemy
    {
        public override void Despawn()
        {
            throw new NotImplementedException();
        }

        public override void Die()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override string GetTextureName(JObject jsonObject)
        {
            throw new NotImplementedException();
        }

        public override void LoadTexture(Texture2D texture)
        {
            throw new NotImplementedException();
        }


        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
