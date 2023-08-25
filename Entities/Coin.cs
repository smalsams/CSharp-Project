using GameAttempt1.Serialization;
using GameAttempt1.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json.Serialization;

namespace GameAttempt1.Entities
{
    [JsonConverter(typeof(EntityConverter))]
    public class Coin : IEntity
    {
        private SpriteStateProcessor _spriteStateProcessor;
        public int Id { get; set; }
        public Vector2 Position { get; set; }
        public CoinType Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Picked { get; set; }
        public RectangleF BoundingBox => new RectangleF(Position, new Size2(Width, Height));

        public Coin(Texture2D texture)
        {
            _spriteStateProcessor = new SpriteStateProcessor();
            _spriteStateProcessor.AddState("i", new HorizontalAnimation(texture, 8, new Point(0, 0), new Point(32, 32)));
            _spriteStateProcessor.ChangeCurrent("i");
            _spriteStateProcessor.Current.Animate();
            (Width, Height) = (texture.Width, texture.Height);
        }
        public void LoadTexture(Texture2D texture)
        {
            _spriteStateProcessor = new SpriteStateProcessor();
            _spriteStateProcessor.AddState("i", new HorizontalAnimation(texture, 8, new Point(0, 0), new Point(32, 32)));
            _spriteStateProcessor.ChangeCurrent("i");
            _spriteStateProcessor.Current.Animate();
            (Width, Height) = (texture.Width, texture.Height);
        }
        public Coin()
        {

        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!Picked)
            {
                _spriteStateProcessor.Draw(spriteBatch, Position, SpriteEffects.None);
            }
        }

        public void PopulateFromJson(JObject jsonObject)
        {
            Type = Enum.Parse<CoinType>(jsonObject["Type"].Value<string>());
            var pos = jsonObject["Position"];
            Position = new Vector2(pos["X"].Value<float>(), pos["Y"].Value<float>());
            Id = jsonObject["Id"].Value<int>();
        }

        public string GetTextureName(JObject jsonObject)
        {
            return $"coin_{jsonObject["Type"].Value<string>()}";
        }

        public void Update(GameTime gameTime)
        {
            _spriteStateProcessor.Update(gameTime);
        }
    }
}