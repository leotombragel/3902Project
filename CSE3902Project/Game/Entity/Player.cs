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
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }
    private bool _previouslyFacingRight = true;
    private float _previousSpeed = 0.0f;
    
    // Vertical motion
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
        CurrentState = new PlayerIdleState(this);
    }
    
    public Player(Sprite sprite)
    {
        Sprite = sprite;
        Velocity = Vector2.Zero;
        Position = new Vector2(StartingPosX, StartingPosY);
        _animationController = new AnimationController(this, new PlaceholderAnimFactory());
        CurrentState = new PlayerIdleState(this);
    }
    
    public void MoveHorizontal(bool isToTheRight)
    {
        // Update speed
        if (Math.Abs(Velocity.X) < MaxSpeed)
        {
            if (isToTheRight != _previouslyFacingRight)
            {
                Velocity = Velocity with { X = 0.0f };
            }
            
            if (isToTheRight)
            {
                Velocity = Velocity with { X = Velocity.X + MoveSpeed };
            }
            else
            {
                Velocity = Velocity with { X = Velocity.X - MoveSpeed };
            }
        }
        
        // Flip the sprite if the direction changes
        if (isToTheRight != _previouslyFacingRight)
        {
            IsFacingLeft = !IsFacingLeft;
        }
        
        _previouslyFacingRight = isToTheRight;
        _previousSpeed = Velocity.X;
    }

    private void Decelerate()
    {
        if (Velocity.X > 0.0f)
        {
            var newSpeed = (float) Math.Max(0.0, Velocity.X - Deceleration);
            Velocity = Velocity with { X = newSpeed };

        }
        else if (Velocity.X < 0.0f)
        {
            var newSpeed = (float) Math.Min(1.0, Velocity.X + Deceleration);
            Velocity = Velocity with { X = newSpeed };
        }
    }
    
    public void MoveVertical()
    {
        if (IsJumping)
        {
            return;
        }

        IsJumping = true;

        // _verticalSpeed = -VerticalMoveSpeed; // Negative makes the player move up (towards the top of the screen where y = 0)
        Velocity = Velocity with { Y = -VerticalMoveSpeed };
    }
    
    private void ApplyGravity()
    {
        if (!IsJumping) return;
        // _verticalSpeed += Gravity;
        Velocity = Velocity with { Y = Velocity.Y + Gravity };

        // Check if the player has landed
        if (Position.Y > StartingPosY)
        {
            Position = new Vector2(Position.X, StartingPosY);
            IsJumping = false;
            Velocity = Velocity with { Y = 0.0f };
        }
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
        _animationController.Update(gameTime);
        Animation?.Update(gameTime);
        CurrentState.Update(gameTime);

        // Decelerate the player when not walking
        Decelerate();
        ApplyGravity();
        UpdatePosition();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animationController.Draw(spriteBatch, Position, IsFacingLeft);
    }

    public bool IsFacingLeft { get; private set; }
    public IState CurrentState { get; set;  }
}