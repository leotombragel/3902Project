using System;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Entity;

public class Player : IAnimatable
{
    // Animation data
    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }
    public bool IsWalking { get; set; }
    public bool IsJumping { get; set; }
    
    private readonly AnimationController _animationController;
    
    // Motion
    private const float MaxSpeed = 3.0f;
    private const float MoveSpeed = 0.15f; // This should be greater than deceleration.
    private const float Deceleration = 0.1f;
    private float _speed = 0.0f;
    private Vector2 Position { get; set; }
    private Vector2 Velocity { get; set; }
    private bool _previouslyFacingRight = true;
    private bool _isFlippedHorizontally;
    private float _previousSpeed = 0.0f;
    
    // Vertical motion
    private float _verticalSpeed = 0.0f;
    private const float VerticalMoveSpeed = 4.0f;
    private const float Gravity = 0.1f;
    
    // Etc
    private const int StartingPosX = 200;
    private const int StartingPosY = 100;

    public Player()
    {
        Sprite = SpriteFactory.Instance.CreateIdlePlayerSprite();
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = Vector2.Zero;
        _animationController = new AnimationController(this, new PlaceholderAnimFactory());
    }
    
    public Player(Sprite sprite)
    {
        Sprite = sprite;
        Velocity = Vector2.Zero;
        Position = new Vector2(StartingPosX, StartingPosY);
        _animationController = new AnimationController(this, new PlaceholderAnimFactory());
    }
    
    public void MoveHorizontal(bool isToTheRight)
    {
        // Update speed
        if (Math.Abs(_speed) < MaxSpeed)
        {
            if (isToTheRight != _previouslyFacingRight)
            {
                _speed = 0.0f;
            }
            
            if (isToTheRight)
            {
                _speed += MoveSpeed;
            }
            else
            {
                _speed -= MoveSpeed;
            }
        }
        
        UpdateHorizontalPosition();
        
        // Flip the sprite if the direction changes
        if (isToTheRight != _previouslyFacingRight)
        {
            _isFlippedHorizontally = !_isFlippedHorizontally;
        }
        
        _previouslyFacingRight = isToTheRight;
        _previousSpeed = _speed;
    }
    
    private void UpdateHorizontalPosition()
    {
        Position = new Vector2(Position.X + _speed, Position.Y);
    }

    private void Decelerate()
    {
        if (_speed > 0.0f)
        {
            // _speed = (float)Math.Max(0.0, _speed - Deceleration);
            Velocity = Velocity with { X = Velocity.X - Deceleration };

        }
        else if (_speed < 0.0f)
        {
            _speed = (float)Math.Min(0.0, _speed + Deceleration);
        }
        UpdateHorizontalPosition();
    }
    
    public void MoveVertical()
    {
        if (IsJumping)
        {
            return;
        }

        IsJumping = true;

        _verticalSpeed = -VerticalMoveSpeed; // Negative makes the player move up (towards the top of the screen where y = 0)
    }
    
    private void ApplyGravity()
    {
        if (!IsJumping) return;
        _verticalSpeed += Gravity;
        UpdateVerticalPosition();

        // Check if the player has landed
        if (Position.Y > StartingPosY)
        {
            Position = new Vector2(Position.X, StartingPosY);
            IsJumping = false;
            _verticalSpeed = 0.0f;
        }
    }
    
    private void UpdateVerticalPosition()
    {
        Position = new Vector2(Position.X, Position.Y + _verticalSpeed);
    }

    /// <summary>
    /// Uses the velocity to update the player's position.
    /// </summary>
    private void UpdatePosition()
    {
        Position += Velocity;
    }

    public virtual void Update(GameTime gameTime)
    {
        Animation?.Update(gameTime);

        // Decelerate the player when not walking
        Decelerate();
        ApplyGravity();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Sprite.Draw(spriteBatch, Position, _isFlippedHorizontally);
    }

    public Direction Facing { get; }
    public IState CurrentState { get; }
}