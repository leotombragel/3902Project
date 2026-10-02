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
    public bool IsFacingLeft { get; private set; }
    public IState CurrentState { get; private set; }
    private readonly AnimationController _animationController;

    // Motion
    private const float MaxSpeed = 3.0f;
    private const float MoveSpeed = 0.15f; // This should be greater than deceleration.
    private const float Deceleration = 0.1f;
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }
    //public Rect Hitbox { get; private set; }
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

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
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


    public void HandleKeeseCollision(KeeseEnemy k)
    {
        float midX = (k.Hitbox.x1 + k.Hitbox.x2) / 2;
        float midY = (k.Hitbox.y1 + k.Hitbox.y2) / 2;
        float slope = (k.Hitbox.y2 - k.Hitbox.y1) / (k.Hitbox.x2 - k.Hitbox.x1);
        float X1 = k.Hitbox.x1;
        float X2 = k.Hitbox.x2;
        float Y1 = k.Hitbox.y1;
        float Y2 = k.Hitbox.y2;

        if(midX < Position.X && Position.X < X2)
        {
            if (-slope * (Position.X - X1) + Y2 < Position.Y && Position.Y < slope * (Position.X - X1) + Y1)
            {
                Velocity = Velocity with { X = 0};
                Position = Position with { X = k.Hitbox.x2 };
            }
        }

        if (midX > Position.X && Position.X > X1)
        {
            if (-slope * (Position.X - X1) + Y2 > Position.Y && Position.Y > slope * (Position.X - X1) + Y1)
            {
                Velocity = Velocity with { X = 0};
                Position = Position with { X = k.Hitbox.x1 };
            }
        }

        if (midY < Position.Y && Position.Y < Y2)
        {
            if ((Position.Y - Y1)/slope + X1 < Position.X && Position.X < -(Position.Y - Y2)/slope + X1)
            {
                Velocity = Velocity with { X = 0, Y = 0 };
                Position = Position with { Y = k.Hitbox.y1 };
            }
        }


        if (midY > Position.Y && Position.Y > Y1)
        {
            if ((Position.Y - Y1) / slope + X1 < Position.X && Position.X < -(Position.Y - Y2) / slope + X1)
            {
                Velocity = Velocity with { Y = 0 };
                Position = Position with { Y = k.Hitbox.y1 };
            }
        }

        if (midY < Position.Y && Position.Y < Y2)
        {
            if ((Position.Y - Y1) / slope + X1 > Position.X && Position.X > -(Position.Y - Y2) / slope + X1)
            {
                Velocity = Velocity with { Y = 0 };
                Position = Position with { Y = k.Hitbox.y2 };
            }
        }



        //if (IsInBoundingBox(k.Hitbox))
        //{


        //if (Velocity.X > 0)
        //{

        //    Velocity = Velocity with { X = 0, Y = 0 };
        //    Position = Position with { X = k.Hitbox.x1 };

        //}

        //else if (Velocity.X < 0)
        //{
        //    Velocity = Velocity with { X = 0, Y = 0 };
        //    Position = Position with { X = k.Hitbox.x2 };             
        //}

        ////test

        //if (Velocity.Y > 0)
        //{

        //    Velocity = Velocity with { X = 0, Y = 0 };
        //    Position = Position with { Y = k.Hitbox.y1 };

        //}

        //else if (Velocity.Y < 0)
        //{
        //    Velocity = Velocity with { Y = 0 };
        //    Position = Position with { Y = k.Hitbox.y2 };
        //    ChangeState(new PlayerIdleState(this));
        //}

        //}
    }

    private bool IsInXRange(Rect r)
    {
        if (r == null)
        {
            return false;
        }
        else if (r.x1 < Position.X && Position.X < r.x2)
            return true;
        else return false;
    }

    private bool IsInYRange(Rect r)
    {
        if (r == null)
        {
            return false;
        }
        else if (r.y1 < Position.Y && Position.Y < r.y2)
            return true;
        else return false;
    }

    private bool IsInBoundingBox(Rect r)
    {
        if(r == null)
        {
            return false;
        }
        return IsInXRange(r) && IsInYRange(r);
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
        ChangeState(new PlayerJumpingState(this));

        // _verticalSpeed = -VerticalMoveSpeed; // Negative makes the player move up (towards the top of the screen where y = 0)
        Velocity = Velocity with { Y = -VerticalMoveSpeed };
    }
    
    private void ApplyGravity()
    {
        if (!IsJumping) return;
        Velocity = Velocity with { Y = Velocity.Y + Gravity };

        if (Velocity.Y > 0.0f)
        {
            ChangeState(new PlayerFallingState(this));
        }

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

    
}