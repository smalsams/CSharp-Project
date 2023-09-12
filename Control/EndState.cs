using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SamSer.Components;

namespace SamSer.Control
{
    /// <summary>
    /// Implementation of the final screen after successfully finishing the last level
    /// </summary>
    public sealed class EndState : State
    {
        /// <remarks>The text shown at the final screen</remarks>
        private TextComponent WinText { get; }
        public EndState(ContentManager contentManager, TwoDPlatformer game, GraphicsDevice graphicsDevice) : base(contentManager, game, graphicsDevice)
        {
            var titleFont = ContentManager.Load<SpriteFont>("TitleFont");
            WinText = new TextComponent(titleFont, new Vector2(600, 400), "You won!", Color.Red);
        }
        /// <inheritdoc/>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            WinText.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            WinText.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Game.Exit();
            }
        }
    }
}
