#region Usings
using System;
using System.Linq;
using GameAttempt1.Control;
using GameAttempt1.Sprites;
using GameAttempt1.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using static GameAttempt1.Utilities.GameUtilities;
#endregion

namespace GameAttempt1.Entities.PlayerContent;

[Serializable]
public class Player : IEntity
{
    #region Fields and Properties
    private readonly Game _game;
    private readonly SpriteStateProcessor _stateProcessor = new();
    public LevelController LevelController;
    public Vector2 Position;
    public Vector2 Velocity;
    public PlayerState State { get; private set; }
    public bool IsAlive { get; private set; }
    public GameDirection Direction { get; private set; }
    public bool HasJumped { get; set; }
    public bool OnPlatform { get; set; }
    public bool Colliding { get; set; }
    public EventHandler Radio { get; set; }
    public int Order { get; set; }
    public int Layer { get; private set; }
    #endregion
    #region Constructors
    public Player(Game game, Texture2D playerTextures)
    {
        _game = game;
        _stateProcessor.AddState(nameof(PlayerTextures.None),
            new Animation(playerTextures, 4, (6, 6), (PLAYER_WIDTH, PLAYER_HEIGHT)));
        _stateProcessor.AddState(nameof(PlayerTextures.Walk),
            new Animation(playerTextures, 6, (46, 6), (PLAYER_WIDTH, PLAYER_HEIGHT)));
        _stateProcessor.AddState(nameof(PlayerTextures.Jump),
            new Animation(playerTextures, 4, (86, 6), (PLAYER_WIDTH, PLAYER_HEIGHT)));
        _stateProcessor.AddState(nameof(PlayerTextures.Swim),
            new Animation(playerTextures, 6, (166, 6), (PLAYER_WIDTH, PLAYER_HEIGHT)));
        _stateProcessor.ChangeCurrent(nameof(PlayerTextures.None));
        _stateProcessor.Current?.Animate();
    }
    #endregion
    #region Player control
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        _stateProcessor.Draw(spriteBatch, Position,
            Direction == GameDirection.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
    }

    public void Update(GameTime gameTime)
    {
        var previousX = Position.X;
        if (State == PlayerState.Paused) return;
        _stateProcessor.Update(gameTime);
        var keyboardState = Keyboard.GetState();
        Position += Velocity;
        if (keyboardState.IsKeyDown(Keys.A))
        {
            Velocity.X = Position.X > PLAYER_WIDTH * 2 ? -DEFAULT_WALK_VELOCITY : 0;
            Direction = GameDirection.Left;
            if (OnPlatform && !HasJumped) Walk();
        }
        else if (keyboardState.IsKeyDown(Keys.D))
        {
            Velocity.X = DEFAULT_WALK_VELOCITY;
            Direction = GameDirection.Right;
            if (OnPlatform && !HasJumped) Walk();
        }
        else
        {
            Velocity.X = 0;
        }

        if (Colliding) Position.X = previousX;
        if (OnPlatform && keyboardState.IsKeyDown(Keys.W) && !HasJumped)
        {
            Position.Y -= JUMP_HEIGHT;
            Velocity.Y -= JUMP_Y_VELOCITY;
            OnPlatform = false;
            Jump();
            HasJumped = true;
            Radio.Invoke(this, EventArgs.Empty);
        }

        if (!OnPlatform)
        {
            Velocity.Y += GRAVITY;
        }
        else
        {
            Velocity.Y = 0;
            HasJumped = false;
        }

        if (Velocity.X == 0 && OnPlatform) Stop();

    }
    public void Pause()
    {
        State = State == PlayerState.Paused ? PlayerState.Playing : PlayerState.Paused;
    }
    #endregion
    #region Player actions
    public void Die()
    {
        IsAlive = false;
    }

    public void Walk()
    {
        _stateProcessor.ChangeCurrent(nameof(PlayerTextures.Walk));
        _stateProcessor.Current.Animate();
    }

    public void Stop()
    {
        _stateProcessor.ChangeCurrent(nameof(PlayerTextures.None));
        _stateProcessor.Current.Animate();
    }

    public void Jump()
    {
        _stateProcessor.ChangeCurrent(nameof(PlayerTextures.Jump));
        _stateProcessor.Current.Animate();
    }

    public void SwimOrCrawl()
    {
        _stateProcessor.ChangeCurrent(nameof(PlayerTextures.Swim));
        _stateProcessor.Current.Animate();
    }
    #endregion
}