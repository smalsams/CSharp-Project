using SamSer.Serialization;
using SamSer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace SamSer.Entities
{
    /// <summary>
    /// Defines a coin object
    /// </summary>
    [JsonConverter(typeof(EntityConverter))]
    public class Coin : IEntity
    {
        #region Fields and Properties
        private SpriteStateProcessor _spriteStateProcessor;
        public int Id { get; set; }
        public Vector2 Position { get; set; }
        public CoinType Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Picked { get; set; }
        public RectangleF BoundingBox => new(Position, new Size2(Width, Height));
        #endregion
        #region Non-Json Constructors
        public Coin(Texture2D texture)
        {
            _spriteStateProcessor = new SpriteStateProcessor();
            _spriteStateProcessor.AddState("i", new HorizontalAnimation(texture, 8, new Point(0, 0), new Point(32, 32)));
            _spriteStateProcessor.ChangeCurrent("i");
            _spriteStateProcessor.Current.Animate();
            (Width, Height) = (texture.Width, texture.Height);
        }
        #endregion
        #region Json Constructors
        public void LoadTexture(Texture2D texture)
        {
            _spriteStateProcessor = new SpriteStateProcessor();
            _spriteStateProcessor.AddState("i", new HorizontalAnimation(texture, 8, new Point(0, 0), new Size(32, 32)));
            _spriteStateProcessor.ChangeCurrent("i");
            _spriteStateProcessor.Current.Animate();
            (Width, Height) = (32, 32);
        }
        public Coin()
        {

        }
        #endregion
        #region Update/Draw Methods
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!Picked)
            {
                _spriteStateProcessor.Draw(spriteBatch, Position, SpriteEffects.None);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!Picked)
            {
                _spriteStateProcessor.Update(gameTime);
            }
        }
        #endregion
        #region Texture Building Methods
        public string GetTextureName(JObject jsonObject)
        {
            return $"coin_{jsonObject["Type"].Value<string>()}";
        }
        #endregion
    }
}