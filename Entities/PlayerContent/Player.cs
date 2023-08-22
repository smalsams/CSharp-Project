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
using Animation = GameAttempt1.Sprites.VerticalAnimation;
using MonoGame.Extended.Tiled;
using System.Reflection.Metadata;

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
    private RectangleF _boundingBox => new(Position.X - 10f, Position.Y +5f, PLAYER_WIDTH, PLAYER_HEIGHT);
    public PlayerState State { get; private set; }
    public GameDirection Direction { get; private set; }
    public bool HasJumped { get; set; }
    public bool CollidingFromTop { get; set; }
    public bool CollidingFromLeft { get; set; }
    public bool CollidingFromBottom { get; set; }
    public bool CollidingFromRight { get; set; }
    public bool InWater { get; set; }
    public EventHandler Radio { get; set; }
    #endregion
    #region Constructors
    public Player(Texture2D playerTextures)
    {
        _stateProcessor.AddState(nameof(PlayerTextures.None),
            new Animation(playerTextures, 4, new Point(6, 6), new Point(PLAYER_WIDTH, PLAYER_HEIGHT), 3));
        _stateProcessor.AddState(nameof(PlayerTextures.Walk),
            new Animation(playerTextures, 6, new Point(46, 6), new Point(PLAYER_WIDTH, PLAYER_HEIGHT), 3));
        _stateProcessor.AddState(nameof(PlayerTextures.Jump),
            new Animation(playerTextures, 4, new Point(86, 6), new Point(PLAYER_WIDTH, PLAYER_HEIGHT), 3));
        _stateProcessor.AddState(nameof(PlayerTextures.Swim),
            new Animation(playerTextures, 6, new Point(166, 6), new Point(PLAYER_HEIGHT, PLAYER_HEIGHT), 3));
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
    public void AirGravityMove(KeyboardState keyboardState, Vector2 previous)
    {
        if (keyboardState.IsKeyDown(Keys.A))
        {
            Velocity.X = Position.X > PLAYER_WIDTH * 2 ? -DEFAULT_WALK_VELOCITY : 0;
            Direction = GameDirection.Left;
            if (CollidingFromTop && !HasJumped) Walk();
        }
        else if (keyboardState.IsKeyDown(Keys.D))
        {
            Velocity.X = DEFAULT_WALK_VELOCITY;
            Direction = GameDirection.Right;
            if (CollidingFromTop && !HasJumped) Walk();
        }
        else
        {
            Velocity.X = 0;
        }
        if (CollidingFromLeft)
        {
            if (previous.X < Position.X)
            {
                _position.X = previous.X;
            }
        }
        if (CollidingFromRight)
        {
            if (previous.X > Position.X)
            {
                _position.X = previous.X;
            }
        }

        if (CollidingFromBottom)
        {
            if (Velocity.Y < 0) Velocity.Y += JUMP_Y_VELOCITY;
            CollidingFromTop = false;
        }
        if (CollidingFromTop && keyboardState.IsKeyDown(Keys.W) && !HasJumped)
        {
            _position.Y -= JUMP_HEIGHT;
            Velocity.Y -= JUMP_Y_VELOCITY;
            CollidingFromTop = false;
            Jump();
            HasJumped = true;
            Radio.Invoke(this, EventArgs.Empty);
        }

        if (!CollidingFromTop)
        {
            if (Velocity.Y < 10f)
            {
                Velocity.Y += GRAVITY;
            }
        }
        else
        {
            Velocity.Y = 0;
            HasJumped = false;
        }

        if (Velocity.X == 0 && CollidingFromTop) Stop();
    }

    public void WaterGravityMove(KeyboardState keyboardState, Vector2 previous)
    {   
        if (keyboardState.IsKeyDown(Keys.W)) { Velocity.Y = -3f; }
        else if (keyboardState.IsKeyDown(Keys.S)) { Velocity.Y = 3f; }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            Direction = GameDirection.Left;
            Velocity.X = Position.X > PLAYER_WIDTH * 2 ? -DEFAULT_WALK_VELOCITY : 0;
        }
        else if (keyboardState.IsKeyDown(Keys.D))
        {
            Direction = GameDirection.Right;
            Velocity.X = DEFAULT_WALK_VELOCITY;
        }
        else
        {
            Velocity.X = 0;
        }

        if (CollidingFromLeft)
        {
            if (previous.X < Position.X)
            {
                _position.X = previous.X;
            }
        }
        else if (CollidingFromRight)
        {
            if (previous.X > Position.X)
            {
                _position.X = previous.X;
            }
        }

        if (CollidingFromBottom)
        {
            if (Velocity.Y < 0) Velocity.Y = -Velocity.Y;
            CollidingFromTop = false;
        }
        if (!CollidingFromTop)
        {
            if (!keyboardState.IsKeyDown(Keys.W) && !keyboardState.IsKeyDown(Keys.S)){ Velocity.Y = 1f; }
            SwimOrCrawl();
        }
        else
        {
            Walk();
            if(!keyboardState.IsKeyDown(Keys.W)) { Velocity.Y = 0; }
        }
        if (Velocity.X == 0 && CollidingFromTop) Stop();
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
        if (InWater)
        {
            WaterGravityMove(keyboardState, previous);
        }
        else
        {
            AirGravityMove(keyboardState, previous);
        }


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
    public void HandleCollisionY(RectangleF platformRectangle, bool platform)
    {
        if (_boundingBox.Right < platformRectangle.Left || _boundingBox.Left > platformRectangle.Right){
            return;

        }
        if (_boundingBox.Bottom - platformRectangle.Top <= 20f && _boundingBox.Bottom < platformRectangle.Bottom && _boundingBox.Bottom > platformRectangle.Top)
        {
            if(_boundingBox.Bottom - platformRectangle.Top > 10f)
            {
                _position.Y -= 10f;
            }
            if (platform)
            {
                CollidingFromTop = true;
            }
        }

        else if (_boundingBox.Top < platformRectangle.Bottom && _boundingBox.Top > platformRectangle.Top &&
                 _boundingBox.Bottom > platformRectangle.Bottom)
        {
            CollidingFromBottom = true;
        }
    }

    public void HandleCollisionX(RectangleF platformRectangle)
    {
        if (_boundingBox.Bottom - 20f < platformRectangle.Top || _boundingBox.Top + 20f> platformRectangle.Bottom)
        {
            return;
        }

        if (_boundingBox.Right  + 5f> platformRectangle.Left && _boundingBox.Right < platformRectangle.Right)
        {
            CollidingFromLeft = true;
        }
        else if (_boundingBox.Left -5f < platformRectangle.Right && _boundingBox.Left > platformRectangle.Left)
        {
            CollidingFromRight = true;
        }
    }

    public void WaterCheck(RectangleF rectangle)
    {
        if (_boundingBox.Intersects(rectangle))
        {
            InWater = true;
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
        CollidingFromTop = false;

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