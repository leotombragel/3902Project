using System;
using CSE3902Project.Game.Entity.State;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Entity;

public class Player : StatefulEntityBase, IAnimatable
{
    // Motion
    private const float MaxSpeed = 3.0f;
    private const float MoveSpeed = 0.15f; // This should be greater than deceleration.
    private const float Deceleration = 0.1f;

    // Vertical motion
    private const float VerticalMoveSpeed = 4.0f;
    private const float Gravity = 0.1f;

    // Etc
    private const int StartingPosX = 200;
    private const int StartingPosY = 100;
    private readonly AnimationController _animationController;
    private bool _previouslyFacingRight = true;
    private float _previousSpeed;

    public Player()
    {
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = Vector2.Zero;
        _animationController = new AnimationController(this, new PlayerAnimationFactory());
        CurrentState = new PlayerIdleState(this);
    }

    /// <summary>
    ///     If true, the player will automatically use its velocity to update its position with gravity and horizontal
    ///     deceleration. Make this false to override the player's position and control it externally.
    /// </summary>
    public bool ShouldManagePosition { get; set; } = true;

    public bool IsWalking { get; set; }
    public bool IsJumping { get; set; }
    public Vector2 Position { get; set; }

    public Vector2 Velocity { get; set; }

    // Animation data
    /// <summary>
    ///     The player's sprite animation. This is also where the player's sprite itself is publicly stored, use
    ///     Animation.Sprite to access this.
    /// </summary>
    public SpriteAnimation Animation { get; set; }

    public bool IsFacingLeft { get; private set; }

    public virtual void Update(GameTime gameTime)
    {
        _animationController.Update(gameTime);
        Animation?.Update(gameTime);
        CurrentState.Update(gameTime);

        if (ShouldManagePosition)
        {
            // Decelerate the player when not walking
            Decelerate();
            ApplyGravity();
        }

        UpdatePosition();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animationController.Draw(spriteBatch, Position, IsFacingLeft);
    }

    public void MoveHorizontal(bool isToTheRight)
    {
        // Update speed
        if (Math.Abs(Velocity.X) < MaxSpeed)
        {
            if (isToTheRight != _previouslyFacingRight) Velocity = Velocity with { X = 0.0f };

            if (isToTheRight)
                Velocity = Velocity with { X = Velocity.X + MoveSpeed };
            else
                Velocity = Velocity with { X = Velocity.X - MoveSpeed };
        }

        // Flip the sprite if the direction changes
        if (isToTheRight != _previouslyFacingRight) IsFacingLeft = !IsFacingLeft;

        _previouslyFacingRight = isToTheRight;
        _previousSpeed = Velocity.X;
    }

    private void Decelerate()
    {
        if (Velocity.X > 0.0f)
        {
            var newSpeed = (float)Math.Max(0.0, Velocity.X - Deceleration);
            Velocity = Velocity with { X = newSpeed };
        }
        else if (Velocity.X < 0.0f)
        {
            var newSpeed = (float)Math.Min(1.0, Velocity.X + Deceleration);
            Velocity = Velocity with { X = newSpeed };
        }
    }

    public void MoveVertical()
    {
        if (IsJumping) return;

        IsJumping = true;
        ChangeState(new PlayerJumpingState(this));

        // _verticalSpeed = -VerticalMoveSpeed; // Negative makes the player move up (towards the top of the screen where y = 0)
        Velocity = Velocity with { Y = -VerticalMoveSpeed };
    }

    private void ApplyGravity()
    {
        if (!IsJumping) return;
        Velocity = Velocity with { Y = Velocity.Y + Gravity };

        // Check if the player has landed
        if (Position.Y > StartingPosY)
        {
            Position = new Vector2(Position.X, StartingPosY);
            IsJumping = false;
            Velocity = Velocity with { Y = 0.0f };
            ChangeState(new PlayerIdleState(this));
        }
    }

    /// <summary>
    ///    Changes the player to the appropriate inactive state depending on whether the player is on the ground, jumping, or falling.
    /// </summary>
    public void SwitchToInactiveState()
    {
        switch (Velocity.Y)
        {
            case > 0:
                ChangeState(new PlayerFallingState(this));
                break;
            case < 0:
                ChangeState(new PlayerJumpingState(this));
                break;
            default:
                ChangeState(new PlayerIdleState(this));
                break;
        }
    }

    /// <summary>
    ///     Uses the velocity to update the player's position.
    /// </summary>
    private void UpdatePosition()
    {
        Position += Velocity;
    }
}