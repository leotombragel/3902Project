using System;
using System.Collections.Generic;
using CSE3902Project.Game.Collision;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Entity.State;
using CSE3902Project.Game.Entity.State.Player_States;
using CSE3902Project.Game.Graphics;
using CSE3902Project.Game.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Entity;

public class Player : IStatefulEntity, IAnimatable, ICombatActor, IUsesItems
{
    // Motion
    private const float MaxSpeed = 3.0f;
    private const float MoveSpeed = 0.15f; // This should be greater than deceleration.
    private const float Deceleration = 0.1f;

    // Vertical motion
    private const float VerticalMoveSpeed = 5.25f;
    private const float Gravity = 0.1f;
    private const float MaxFallSpeed = 8.0f;

    // Stage collision (walking on tiles)
    // the box is smaller than the 64x64 sprite and moves over when the player turns around
    private const int HitboxWidth = 24;
    private const int HitboxHeight = 30;
    private const int HitboxTop = 32;

    // Etc
    private const int StartingPosX = 200;
    private readonly AnimationController _animationController;
    public int Health { get; set; } = 3;
    public int Rupees { get; set; } = 0;
    public bool FinishedLevel { get; set; }

    // Collision
    public readonly IHurtbox _bodyHurtbox;
    private int _iFramesBuffer = 0; //invincibilty frames
    private readonly List<IItem> _items = new();
    private bool _previouslyFacingRight = true;
    private float _previousSpeed;
    private PlayerStageCollision _stageCollision;

    public Player(int groundHeight)
    {
        StartingPosY =
            groundHeight - 32 -
            32; // Assuming the player sprite is 32 pixels tall and we want it to be above the ground tile
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = Vector2.Zero;
        _animationController = new AnimationController(this, new PlayerAnimationFactory());
        CurrentState = new PlayerIdleState(this);
        _bodyHurtbox = new BodyHurtbox(this, new Point(8, 31), new Point(32, 32));
    }

    // Animation data
    public Sprite Sprite { get; set; }

    /// <summary>
    ///     If true, the player will automatically use its velocity to update its position with gravity and horizontal
    ///     deceleration. Make this false to override the player's position and control it externally.
    /// </summary>
    public bool ShouldManagePosition { get; set; } = true;

    public bool AcceptsStateChanges { get; set; } = true;

    public bool IsWalking { get; set; }
    public bool IsJumping { get; set; }
    public bool IsOnGround { get; set; }
    public Vector2 Velocity { get; set; }
    public int StartingPosY { get; } = 100;

    /// the box used for walking on tiles (smaller than the whole sprite)
    public Rectangle StageHitbox
    {
        get
        {
            var x = (int)Math.Round(Position.X) + 11;
            var y = (int)Math.Round(Position.Y) + HitboxTop;
            return new Rectangle(x, y, HitboxWidth, HitboxHeight);
        }
    }

    public bool IsUsingBow { get; set; }

    public SpriteAnimation Animation { get; set; }

    public virtual void Update(GameTime gameTime)
    {
        var wasOnGround = IsOnGround;

        _animationController.Update(gameTime);
        Animation?.Update(gameTime);
        CurrentState.Update(gameTime);

        if(!_bodyHurtbox.IsEnabled) //invincibility frames
        {
            _iFramesBuffer += 1;
            if(_iFramesBuffer > 60)
            {
                _bodyHurtbox.IsEnabled = true;
                _iFramesBuffer = 0;
            }
        }

        if (ShouldManagePosition)
        {
            // Decelerate the player when not walking
            Decelerate();
            ApplyGravity();
        }

        if (IsDead) ChangeState(new PlayerDeadState(this));

        UpdatePosition();

        if (!wasOnGround && IsOnGround) IsFacingLeft = !_previouslyFacingRight;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animationController.Draw(spriteBatch, Position, IsFacingLeft);
    }

    public bool IsFacingLeft { get; private set; }

    public IEnumerable<IHurtbox> Hurtboxes
    {
        get { yield return _bodyHurtbox; }
    }

    public Vector2 Position { get; set; }
    public DamageTeam Team => DamageTeam.Player;

    public bool IsDead { get; set; }

    public void TakeDamage(DamageInfo damageInfo)
    {
        Health -= 1;
        if (CurrentItem is not null)
        {
            CurrentItem = null;
        }
        
        Console.WriteLine($"Player took damage. Source: {damageInfo.Source}");
        Console.WriteLine($"Player health: {Health}");
        _bodyHurtbox.IsEnabled = false;

        if (Health <= 0)
        {
            IsDead = true;
        }
    }

    public void FlipDirection()
    {
        Velocity = new Vector2(-Velocity.X, Velocity.Y);
        IsFacingLeft = !IsFacingLeft;
    }

    public IState CurrentState { get; private set; }

    public void ChangeState(IState newState)
    {
        // Ignore state change if re-entering the current state
        if (newState.GetType() == CurrentState.GetType() || !AcceptsStateChanges) return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public IItem CurrentItem { get; private set; }

    public void InteractWithItem(IItem item)
    {
        if (IsUsingBow) 
        {//state changes get buggy if we switch to a different weapon while player is charging bow
            if(item is not RupeeItem
            && item is not HeartItem
            && item is not KeyItem
            && item is not ArrowItem
            && item is not BowItem){
                IsUsingBow = false;
                SwitchToInactiveState();
            }
        }

        switch (item)
        {
            //special cases for item interactions
            //need to use bow or arrow item. for now, both give bow item
            case ArrowItem:
                CurrentItem = new BowItem(item.Position);
                return;
            case RupeeItem:
                Rupees += 1;
                return;
            case HeartItem:
                Health += 1;
                return;
            case KeyItem:
                return; //we have no use for the key yet
            case TriforceItem:
                ChangeState(new PlayerTriforceState(this));
                return;
            default:
                //item is bomb, boomerang or fire
                CurrentItem = item;
                break;
        }
    }

    public void SetStageCollision(PlayerStageCollision stageCollision)
    {
        _stageCollision = stageCollision;
    }

    public void MoveHorizontal(bool isToTheRight)
    {
        var directionChanged = isToTheRight != _previouslyFacingRight;

        // Update speed
        if (Math.Abs(Velocity.X) < MaxSpeed)
        {
            if (isToTheRight != _previouslyFacingRight) Velocity = Velocity with { X = 0.0f };

            if (isToTheRight)
                Velocity = Velocity with { X = Velocity.X + MoveSpeed };
            else
                Velocity = Velocity with { X = Velocity.X - MoveSpeed };
        }

        if (IsOnGround && directionChanged) IsFacingLeft = !isToTheRight;

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
        //can only jump when standing on something
        if (!IsOnGround) return;

        IsOnGround = false;
        IsJumping = true;
        if (!IsUsingBow)
            ChangeState(new PlayerJumpingState(this));
        else
            ChangeState(new PlayerBowJumpState(this));

        // _verticalSpeed = -VerticalMoveSpeed; // Negative makes the player move up (towards the top of the screen where y = 0)
        Velocity = Velocity with { Y = -VerticalMoveSpeed };
    }

    public void TeleportTo(Vector2 position)
    {
        Position = position;
        Velocity = Vector2.Zero;
        IsOnGround = false;
        IsJumping = false;
        SwitchToInactiveState();
    }

    private void ApplyGravity()
    {
        //no gravity while standing on something
        if (IsOnGround)
        {
            if (CurrentState is PlayerFallingState or PlayerBowFallState) SwitchToInactiveState();
            return;
        }

        //the dead state does its own gravity
        if (CurrentState is PlayerDeadState) return;

        var newSpeed = Velocity.Y + Gravity;
        if (newSpeed > MaxFallSpeed) newSpeed = MaxFallSpeed;

        Velocity = Velocity with { Y = newSpeed };
    }

    public void ReleaseBow()
    {
        IsUsingBow = false;
        SwitchToInactiveState();
    }

    /// <summary>
    ///     Changes the player to the appropriate inactive state depending on whether the player is on the ground, jumping, or
    ///     falling.
    /// </summary>
    public void SwitchToInactiveState()
    {
        if (IsUsingBow)
            switch (Velocity.Y)
            {
                case > 0:
                    ChangeState(new PlayerBowFallState(this));
                    return;
                case < 0:
                    ChangeState(new PlayerBowJumpState(this));
                    return;
                default:
                    ChangeState(new PlayerBowIdleState(this));
                    return;
            }

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
        //dead players fall through the stage, so skip the collision
        if (_stageCollision == null || !ShouldManagePosition || CurrentState is PlayerDeadState)
        {
            IsOnGround = false;
            Position += Velocity;
            return;
        }

        _stageCollision.MovePlayer(this);
    }
}