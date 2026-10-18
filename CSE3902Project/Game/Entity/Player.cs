using System;
using System.Collections.Generic;
using CSE3902Project.Game.Collision;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Entity.State;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Entity;

public class Player : StatefulEntityBase, IAnimatable, ICombatActor
{
    // Motion
    private const float MaxSpeed = 3.0f;
    private const float MoveSpeed = 0.15f; // This should be greater than deceleration.
    private const float Deceleration = 0.1f;

    // Vertical motion
    private const float VerticalMoveSpeed = 4.0f;
    private const float Gravity = 0.1f;
    
    // Collision
    private readonly IHurtbox _bodyHurtbox;

    public IEnumerable<IHurtbox> Hurtboxes
    {
        get
        {
            yield return _bodyHurtbox;
        }
    }

    // Etc
    private const int StartingPosX = 200;
    private readonly AnimationController _animationController;
    private bool _previouslyFacingRight = true;
    private float _previousSpeed;

    public Player(int groundHeight)
    {
        StartingPosY =
            groundHeight - 32 -
            32; // Assuming the player sprite is 32 pixels tall and we want it to be above the ground tile
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = Vector2.Zero;
        _animationController = new AnimationController(this, new PlayerAnimationFactory());
        CurrentState = new PlayerIdleState(this);
        _bodyHurtbox = new BodyHurtbox(this, new Point(8, 8), new Point(32, 32)); // TODO
    }

    // Animation data
    public Sprite Sprite { get; set; }

    /// <summary>
    ///     If true, the player will automatically use its velocity to update its position with gravity and horizontal
    ///     deceleration. Make this false to override the player's position and control it externally.
    /// </summary>
    public bool ShouldManagePosition { get; set; } = true;

    public bool IsWalking { get; set; }
    public bool IsJumping { get; set; }
    public Vector2 Velocity { get; set; }
    public int StartingPosY { get; } = 100;

    public Vector2 Position { get; set; }
    public DamageTeam Team => DamageTeam.Player;

    public bool IsDead { get; set; }
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

    public void TakeDamage(DamageInfo damageInfo)
    {
        Console.WriteLine($"Player took damage. Source: {damageInfo.Source}");
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


    // public void HandleKeeseCollision(KeeseEnemy k)
    // {
    //     var midX = (k.Hitbox.x1 + k.Hitbox.x2) / 2;
    //     var midY = (k.Hitbox.y1 + k.Hitbox.y2) / 2;
    //     var slope = (k.Hitbox.y2 - k.Hitbox.y1) / (k.Hitbox.x2 - k.Hitbox.x1);
    //     var X1 = k.Hitbox.x1;
    //     var X2 = k.Hitbox.x2;
    //     var Y1 = k.Hitbox.y1;
    //     var Y2 = k.Hitbox.y2;
    //
    //     if (midX < Position.X && Position.X < X2)
    //         if (-slope * (Position.X - X1) + Y2 < Position.Y && Position.Y < slope * (Position.X - X1) + Y1)
    //         {
    //             Velocity = Velocity with { X = 0 };
    //             Position = Position with { X = k.Hitbox.x2 };
    //         }
    //
    //     if (midX > Position.X && Position.X > X1)
    //         if (-slope * (Position.X - X1) + Y2 > Position.Y && Position.Y > slope * (Position.X - X1) + Y1)
    //         {
    //             Velocity = Velocity with { X = 0 };
    //             Position = Position with { X = k.Hitbox.x1 };
    //         }
    //
    //     if (midY < Position.Y && Position.Y < Y2)
    //         if ((Position.Y - Y1) / slope + X1 < Position.X && Position.X < -(Position.Y - Y2) / slope + X1)
    //         {
    //             Velocity = Velocity with { X = 0, Y = 0 };
    //             Position = Position with { Y = k.Hitbox.y1 };
    //         }
    //
    //
    //     if (midY > Position.Y && Position.Y > Y1)
    //         if ((Position.Y - Y1) / slope + X1 < Position.X && Position.X < -(Position.Y - Y2) / slope + X1)
    //         {
    //             Velocity = Velocity with { Y = 0 };
    //             Position = Position with { Y = k.Hitbox.y1 };
    //         }
    //
    //     if (midY < Position.Y && Position.Y < Y2)
    //         if ((Position.Y - Y1) / slope + X1 > Position.X && Position.X > -(Position.Y - Y2) / slope + X1)
    //         {
    //             Velocity = Velocity with { Y = 0 };
    //             Position = Position with { Y = k.Hitbox.y2 };
    //         }
    // }
    //
    // private bool IsInXRange(Rect r)
    // {
    //     if (r == null) return false;
    //
    //     if (r.x1 < Position.X && Position.X < r.x2)
    //         return true;
    //     return false;
    // }
    //
    // private bool IsInYRange(Rect r)
    // {
    //     if (r == null) return false;
    //
    //     if (r.y1 < Position.Y && Position.Y < r.y2)
    //         return true;
    //     return false;
    // }
    //
    // private bool IsInBoundingBox(Rect r)
    // {
    //     if (r == null) return false;
    //     return IsInXRange(r) && IsInYRange(r);
    // }


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
    ///     Changes the player to the appropriate inactive state depending on whether the player is on the ground, jumping, or
    ///     falling.
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