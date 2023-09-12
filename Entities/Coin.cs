using SamSer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework.Audio;

namespace SamSer.Entities
{
    /// <summary>
    /// Defines a collectable coin object used as a score measurement
    /// </summary>
    public class Coin : IEntity
    {
        #region Fields and Properties
        private SpriteStateProcessor _spriteStateProcessor;
        public int Id { get; set; }
        public Vector2 Position { get; set; }
        public CoinType Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public RectangleF BoundingBox => new(Position, new Size2(Width, Height));

        public bool Paused { get; set; }
        public bool InSight { get ; set; }
        #endregion
        #region Non-Json Constructors
        public Coin(Texture2D texture)
        {
            LoadTexture(texture);
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
            if (InSight)
            {
                _spriteStateProcessor.Draw(spriteBatch, Position, SpriteEffects.None);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (InSight && !Paused)
            {
                _spriteStateProcessor.Update(gameTime);
            }
        }
        #endregion
        #region Asset Building Methods
        public string GetTextureName(JObject jsonObject)
        {
            var res = int.TryParse(jsonObject["Type"].Value<string>(), out var intResult);
            return res ? $"coin_{(CoinType)(intResult)}" : $"coin_{jsonObject["Type"].Value<string>()}";
        }

        #endregion
    }
}