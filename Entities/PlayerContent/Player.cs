#region Usings
using SamSer.Control;
using SamSer.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static SamSer.Utilities.GameUtilities;
using Animation = SamSer.Sprites.VerticalAnimation;

#endregion

namespace SamSer.Entities.PlayerContent;

/// <summary>
/// The playable character for the game.
/// </summary>
public class Player : IFocusable
{
    #region Fields and Properties

    private Texture2D _rectangleTexture2D;
    private bool _drawDebug;
    private readonly SpriteStateProcessor _stateProcessor = new();
    public float PlayerDefaultX = 100;
    public float PlayerDefaultY = 600;
    /// <remarks>Determines the current position of the <see cref="Player"/> on the screen</remarks>
    public Vector2 Position { get => _position; set => _position = value; }
    public Vector2 Velocity;
    private Vector2 _position;
    /// <remarks>Determines the current hitboxes of the <see cref="Player"/>.</remarks>
    private RectangleF _boundingBox => new(Position.X - COLLISION_THRESHOLD_Y, Position.Y + COLLISION_THRESHOLD_X,
        PLAYER_WIDTH, PLAYER_HEIGHT);
    /// <remarks>Time for which the <see cref="Player"/> cannot die (in seconds)</remarks>>
    public float InvulnerabilityDuration = 0.5f;
    /// <remarks>Time for which the <see cref="Player"/> has been invulnerable.</remarks>
    public float InvulnerabilityTimer;
    public PlayerState State { get; private set; }
    /// <remarks>The direction the <see cref="Player"/> is facing.</remarks>
    public GameDirection Direction { get; private set; }
    /// <remarks>Determines whether the <see cref="Player"/> has recently jumped</remarks>
    public bool HasJumped { get; set; }
    //colliders
    public bool CollidingFromTop { get; set; }
    public bool CollidingFromLeft { get; set; }
    public bool CollidingFromBottom { get; set; }
    public bool CollidingFromRight { get; set; }
    /// <remarks>Indicates whether the <see cref="Player"/> is in a water environment</remarks>
    public bool InWater { get; set; }
    /// <remarks>Current health points of the <see cref="Player"/>.</remarks>
    public int Health { get; set; }
    /// <summary>
    /// Invoked at <see cref="Player"/> jumping.
    /// </summary>
    public EventHandler JumpEvent { get; set; }
    /// <summary>
    /// Invoked at <see cref="Player"/> having less than 0 health
    /// </summary>
    public EventHandler NoHealthEvent { get; set; }


    public RectangleF BoundingBox => _boundingBox;

    public static int Id { get => 1; set => _ = 1; }
    #endregion
    #region Constructors

    public Player(Texture2D playerTextures)
    {
        _stateProcessor.AddState(nameof(PlayerTextures.None),
    new Animation(playerTextures, PLAYER_WALK_ANI_COUNT, new Point(PLAYER_TEXTURE_DEFAULT, PLAYER_TEXTURE_DEFAULT),
        new Size(PLAYER_WIDTH, PLAYER_HEIGHT), 3));
        _stateProcessor.AddState(nameof(PlayerTextures.Walk),
            new Animation(playerTextures, PLAYER_JUMP_ANI_COUNT,
                new Point(PLAYER_TEXTURE_DEFAULT + PLAYER_TEXTURE_X_OFFSET, PLAYER_TEXTURE_DEFAULT),
                new Size(PLAYER_WIDTH, PLAYER_HEIGHT), 3));
        _stateProcessor.AddState(nameof(PlayerTextures.Jump),
            new Animation(playerTextures, PLAYER_SWIM_ANI_COUNT,
                new Point(PLAYER_TEXTURE_DEFAULT + 2 * PLAYER_TEXTURE_X_OFFSET, PLAYER_TEXTURE_DEFAULT),
                new Size(PLAYER_WIDTH, PLAYER_HEIGHT), 3));
        _stateProcessor.AddState(nameof(PlayerTextures.Swim),
            new Animation(playerTextures, PLAYER_IDLE_ANI_COUNT,
                new Point(PLAYER_TEXTURE_DEFAULT + 4 * PLAYER_TEXTURE_X_OFFSET, PLAYER_TEXTURE_DEFAULT),
                new Size(PLAYER_HEIGHT, PLAYER_HEIGHT), 3));
        _stateProcessor.ChangeCurrent(nameof(PlayerTextures.None));
        _stateProcessor.Current?.Animate();
        Position = new Vector2(PlayerDefaultX, PlayerDefaultY);
        Health = 3;
    }
    [JsonConstructor]
    public Player(PlayerData data)
    {
        Position = data.Position;
        State = data.State;
        Direction = data.Direction;
        Health = 3;
    }
    /// <summary>
    /// Loads all necessary assets for the <see cref="Player"/> and constructs the necessary components for <see cref="Animation"/>
    /// </summary>
    /// <param name="spriteSheet">The texture with all sprites necessary for <see cref="Player"/> construction</param>
    public void LoadTexture(Texture2D spriteSheet)
    {
        _stateProcessor.AddState(nameof(PlayerTextures.None),
            new Animation(spriteSheet, PLAYER_WALK_ANI_COUNT, new Point(PLAYER_TEXTURE_DEFAULT, PLAYER_TEXTURE_DEFAULT),
                new Size(PLAYER_WIDTH, PLAYER_HEIGHT), GAME_INCR_SCALE));
        _stateProcessor.AddState(nameof(PlayerTextures.Walk),
            new Animation(spriteSheet, PLAYER_JUMP_ANI_COUNT,
                new Point(PLAYER_TEXTURE_DEFAULT + PLAYER_TEXTURE_X_OFFSET, PLAYER_TEXTURE_DEFAULT),
                new Size(PLAYER_WIDTH, PLAYER_HEIGHT), GAME_INCR_SCALE));
        _stateProcessor.AddState(nameof(PlayerTextures.Jump),
            new Animation(spriteSheet, PLAYER_SWIM_ANI_COUNT,
                new Point(PLAYER_TEXTURE_DEFAULT + 2 * PLAYER_TEXTURE_X_OFFSET, PLAYER_TEXTURE_DEFAULT),
                new Size(PLAYER_WIDTH, PLAYER_HEIGHT), GAME_INCR_SCALE));
        _stateProcessor.AddState(nameof(PlayerTextures.Swim),
            new Animation(spriteSheet, PLAYER_IDLE_ANI_COUNT,
                new Point(PLAYER_TEXTURE_DEFAULT + 4 * PLAYER_TEXTURE_X_OFFSET, PLAYER_TEXTURE_DEFAULT),
                new Size(PLAYER_HEIGHT, PLAYER_HEIGHT), GAME_INCR_SCALE));
        _stateProcessor.ChangeCurrent(nameof(PlayerTextures.None));
        _stateProcessor.Current?.Animate();
    }
    #endregion
    #region Player control
    /// <inheritdoc cref="IEntity.Draw"/>
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        _stateProcessor.Draw(spriteBatch, Position,
            Direction == GameDirection.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        if (_drawDebug)
        {
            spriteBatch.Draw(_rectangleTexture2D, _boundingBox.TopLeft, Color.Purple);
        }
    }
    /// <summary>
    /// Defines movement and physics in air environment, checks <see cref="Player"/> input and invokes movement events.
    /// </summary>
    /// <param name="keyboardState">Keys that are currently pressed</param>
    /// <param name="previous">Last position of the player</param>
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
            JumpEvent.Invoke(this, EventArgs.Empty);
        }

        if (!CollidingFromTop)
        {
            if (Velocity.Y < GRAVITY_LIMIT)
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
    /// <summary>
    /// Defines movement and physics in water environment, checks <see cref="Player"/> input and invokes movement events.
    /// </summary>
    /// <param name="keyboardState">Keys that are currently pressed</param>
    /// <param name="previous">Last <see cref="Position"/> of the <see cref="Player"/></param>
    public void WaterGravityMove(KeyboardState keyboardState, Vector2 previous)
    {
        if (keyboardState.IsKeyDown(Keys.W)) { Velocity.Y = -WATER_RUSH_VELOCITY; }
        else if (keyboardState.IsKeyDown(Keys.S)) { Velocity.Y = WATER_RUSH_VELOCITY; }
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
            if (!keyboardState.IsKeyDown(Keys.W) && !keyboardState.IsKeyDown(Keys.S)) { Velocity.Y = WATER_DEFAULT_VELOCITY; }
            Swim();
        }
        else
        {
            Walk();
            if (!keyboardState.IsKeyDown(Keys.W)) { Velocity.Y = 0; }
        }
        if (Velocity.X == 0 && CollidingFromTop) Stop();
    }
    /// <inheritdoc cref="IEntity.Update"/>
    public void Update(GameTime gameTime)
    {
        var previous = Position;
        if (State == PlayerState.Paused) return;
        _stateProcessor.Update(gameTime);
        var keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.F1))
        {
            _drawDebug = !_drawDebug;
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
        if (InvulnerabilityTimer > 0f)
        {
            InvulnerabilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

    }
    /// <summary>
    /// Draws lines around the <see cref="Player"/>, mainly for debug and test purposes
    /// </summary>
    /// <param name="gd"></param>
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
                    colours.Add(new Color(255, 255, 255, 255));
                }
                else
                {
                    colours.Add(new Color(0, 0, 0, 0));
                }
            }
        }
        _rectangleTexture2D.SetData(colours.ToArray());
    }
    /// <summary>
    /// Handles collision with terrain on Y-axis, basically only controlling vertical movement
    /// </summary>
    /// <param name="platformRectangle"></param>
    /// <param name="platform"></param>
    public void HandleCollisionY(RectangleF platformRectangle, bool platform)
    {
        if(_boundingBox.Top - PLAYER_WIDTH< 0)
        {
            CollidingFromBottom = true;
        }
        if (_boundingBox.Right < platformRectangle.Left || _boundingBox.Left > platformRectangle.Right)
        {
            return;
        }
        if (_boundingBox.Bottom - platformRectangle.Top <= PLAYER_HEIGHT && _boundingBox.Bottom < platformRectangle.Bottom && _boundingBox.Bottom > platformRectangle.Top)
        {
            if (_boundingBox.Bottom - platformRectangle.Top > COLLISION_THRESHOLD_Y)
            {
                _position.Y -= COLLISION_THRESHOLD_Y;
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
    /// <summary>
    /// Handles collision with terrain on X-axis, basically only controlling horizontal movement
    /// </summary>
    /// <param name="platformRectangle"></param>
    public void HandleCollisionX(RectangleF platformRectangle)
    {
        if (_boundingBox.Bottom - PLAYER_HEIGHT < platformRectangle.Top || _boundingBox.Top + PLAYER_HEIGHT > platformRectangle.Bottom)
        {
            return;
        }

        if (_boundingBox.Right + COLLISION_THRESHOLD_X > platformRectangle.Left && _boundingBox.Right < platformRectangle.Right)
        {
            CollidingFromLeft = true;
        }
        else if (_boundingBox.Left - COLLISION_THRESHOLD_X < platformRectangle.Right && _boundingBox.Left > platformRectangle.Left)
        {
            CollidingFromRight = true;
        }
    }
    /// <summary>
    /// Checks if the <see cref="Player"/> is located in water
    /// </summary>
    /// <param name="rectangle"></param>
    public void WaterCheck(RectangleF rectangle)
    {
        if (_boundingBox.Intersects(rectangle))
        {
            InWater = true;
        }
    }
    /// <summary>
    /// Pauses the <see cref="Player"/> animation
    /// </summary>
    public void Pause()
    {
        State = State == PlayerState.Paused ? PlayerState.Playing : PlayerState.Paused;
    }
    /// <summary>
    /// Makes the <see cref="Player"/> reset its position and removes 1 health point
    /// </summary>
    public void Die()
    {
        Position = new Vector2(PlayerDefaultX, PlayerDefaultY);
        HasJumped = true;
        CollidingFromTop = false;
        InvulnerabilityTimer = InvulnerabilityDuration;
        Health--;
        if(Health < 0)
        {
            NoHealthEvent.Invoke(this, EventArgs.Empty);
        }

    }
    /// <summary>
    /// Resets every collider booleans
    /// </summary>
    public void ResetPhysicsValues()
    {
        InWater = false;
        CollidingFromBottom = false;
        CollidingFromLeft = false;
        CollidingFromTop = false;
        CollidingFromRight = false;
    }
    #endregion
    #region Player actions
    /// <summary>
    /// Makes the <see cref="Player"/>'s animation be a Walk animation
    /// </summary>
    public void Walk()
    {
        _stateProcessor.ChangeCurrent(nameof(PlayerTextures.Walk));
        _stateProcessor.Current.Animate();
    }
    /// <summary>
    /// Makes the <see cref="Player"/>'s animation be an Idle animation
    /// </summary>
    public void Stop()
    {
        _stateProcessor.ChangeCurrent(nameof(PlayerTextures.None));
        _stateProcessor.Current.Animate();
    }
    /// <summary>
    /// Makes the <see cref="Player"/>'s animation be a Jump animation
    /// </summary>
    public void Jump()
    {
        _stateProcessor.ChangeCurrent(nameof(PlayerTextures.Jump));
        _stateProcessor.Current.Animate();
    }
    /// <summary>
    /// Makes the <see cref="Player"/>'s animation be a Swim animation
    /// </summary>
    public void Swim()
    {
        _stateProcessor.ChangeCurrent(nameof(PlayerTextures.Swim));
        _stateProcessor.Current.Animate();
    }

    /// <summary>
    /// Gets the texture name for <see cref="Player"/>
    /// </summary>
    /// <returns>The name of the texture exclusive for <see cref="Player"/></returns>
    public static string GetTextureName()
    {
        return PLAYER_TEXTURE_NAME;
    }



    #endregion
}