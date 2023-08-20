#region Usings
using GameAttempt1.Control;
using GameAttempt1.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using static GameAttempt1.Utilities.GameUtilities;
using Animation = GameAttempt1.Sprites.Animation;

#endregion

namespace GameAttempt1.Entities.PlayerContent;

[Serializable]
public class Player : IEntity, IFocusable
{
    #region Fields and Properties

    private Texture2D _rectangleTexture2D;
    private bool drawDebug;
    private readonly SpriteStateProcessor _stateProcessor = new();
    public Vector2 Position { get => _position ; set => _position = value; }
    public Vector2 Velocity;
    private Vector2 _position;
    private RectangleF _boundingBox => new(Position.X, Position.Y, PLAYER_WIDTH, PLAYER_HEIGHT);
    public PlayerState State { get; private set; }
    public GameDirection Direction { get; private set; }
    public bool HasJumped { get; set; }
    public bool OnPlatform { get; set; }
    public bool CollidingX { get; set; }
    public bool CollidingY { get; set; }
    public EventHandler Radio { get; set; }
    #endregion
    #region Constructors
    public Player(Texture2D playerTextures)
    {
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
        if (drawDebug)
        {
            spriteBatch.Draw(_rectangleTexture2D, _boundingBox.TopLeft, Color.Purple);
        }
    }

    public void Update(GameTime gameTime)
    {
        var previous = Position;
        if (State == PlayerState.Paused) return;
        _stateProcessor.Update(gameTime);
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.F1))
        {
            drawDebug = !drawDebug;
        }
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

        if (CollidingX){ _position.X = previous.X;}

        if (CollidingY)
        {
            if (Velocity.Y < 0) Velocity.Y += JUMP_Y_VELOCITY;
        }
        if (OnPlatform && keyboardState.IsKeyDown(Keys.W) && !HasJumped)
        {
            _position.Y -= JUMP_HEIGHT;
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

    public void DrawDebug(GraphicsDevice gd)
    {
        _rectangleTexture2D = new Texture2D(gd, (int)_boundingBox.Width, (int)_boundingBox.Height);
        var colours = new List<Color>();
        for (var i = 0; i < (int)_boundingBox.Height; i++)
        {
            for (var j = 0; j < (int)_boundingBox.Width; j++)
            {
                if (i == 0 || j == 0 || i == (int)_boundingBox.Height - 1 || j == (int)_boundingBox.Width - 1)
                {
                    colours.Add(new Color(255,255,255,255));
                }
                else
                {
                    colours.Add(new Color(0,0,0,0));
                }
            }
        }
        _rectangleTexture2D.SetData(colours.ToArray());
    }
    public void HandleCollisionY(RectangleF platformRectangle)
    {
        if (_boundingBox.Right < platformRectangle.Left || _boundingBox.Left > platformRectangle.Right){
            return;

        }
        if (_boundingBox.Bottom > platformRectangle.Top && _boundingBox.Bottom < platformRectangle.Bottom && _boundingBox.Top < platformRectangle.Top)
        {
            OnPlatform = true;
        }

        else if (_boundingBox.Top < platformRectangle.Bottom && _boundingBox.Top > platformRectangle.Top && _boundingBox.Bottom > platformRectangle.Bottom)
        {
            CollidingY = true;
        }
    }

    public void HandleCollisionX(RectangleF platformRectangle)
    {
        if (_boundingBox.Bottom - 10f < platformRectangle.Top || _boundingBox.Top > platformRectangle.Bottom)
        {
            return;
        }

        if (_boundingBox.Right > platformRectangle.Left && _boundingBox.Right < platformRectangle.Right ||
            _boundingBox.Left < platformRectangle.Right && _boundingBox.Left > platformRectangle.Left)
        {
            CollidingX = true;
        }
    }

    public void HandleCollisionY(Rectangle rectangle)
    {
        if (!(_boundingBox.Left > rectangle.Left) || !(_boundingBox.Right < rectangle.Right)) return;
        if (_boundingBox.Top < rectangle.Bottom)
        {
            CollidingY = true;
            return;
        }

        if (_boundingBox.Bottom > rectangle.Top)
        {
            OnPlatform = true;
        }
    }
    public void HandleCollisionX(Rectangle rectangle)
    {
        if (!(_boundingBox.Bottom > rectangle.Top) || !(_boundingBox.Top < rectangle.Bottom)) return;
        if (_boundingBox.Left > rectangle.Left || _boundingBox.Right < rectangle.Right)
        {
            CollidingX = true;
        }
    }

    public void Pause()
    {
        State = State == PlayerState.Paused ? PlayerState.Playing : PlayerState.Paused;
    }

    public void Reset()
    {
        Position = new Vector2(PLAYER_DEFAULT_X, PLAYER_DEFAULT_Y);
        HasJumped = true;
        OnPlatform = false;

    }
    #endregion
    #region Player actions

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