using System;
using System.Collections.Generic;
using CSE3902Project.Game.Collision;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Entity.State;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Entity;

/// <summary>
///     keese enemy class, implements StatefulEntityBase, IEnemy
/// </summary>
public class KeeseEnemy : StatefulEntityBase, IEnemy
{
    //movement
    public Vector2 BoxContainer { get; } //constricting values so keese doesn't fly off the screen
    private static readonly Random _rng = new();
    public Vector2 Position { get; private set; }

    //animation
    private readonly AnimationController _animationController;
    public IHurtbox _bodyHurtbox { get;}
     private readonly int StartingPosX = 400; //csv file will override this
    private readonly int StartingPosY = 100; //csv file will override this
    public Sprite Sprite { get; set; }
    public bool IsGoingUp { get; private set; }
    public Vector2 Velocity { get; private set; }
    public SpriteAnimation Animation { get; set; }

    //state
    public bool IsFacingLeft { get; private set; }
    private int deadAnimationBuffer = 0;
    public bool IsDead { get; set; }

    public KeeseEnemy(int GroundHeight)
    {
        Sprite = SpriteFactory.Instance.CreateKeeseSprite();
        var spriteHeight = Sprite.SourceRectangle?.Height ?? 32;
        StartingPosY =
            GroundHeight - 32 -
            spriteHeight; // Assuming the keese sprite is 32 pixels tall and we want it to be above the ground tile

        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = new Vector2(-1, 1);
        BoxContainer = new Vector2(100, 50);
        IsGoingUp = false;
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new KeeseAnimationFactory());
        CurrentState = new KeeseFlyingState(this);
        _bodyHurtbox = new BodyHurtbox(this, new Point(0, 0), new Point(32, 26));
    }

    public KeeseEnemy(int groundHeight, Vector2 spawnPosition)
        : this(groundHeight)
    {
        StartingPosY = (int)spawnPosition.Y;
        StartingPosX = (int)spawnPosition.X;
        Position = spawnPosition;
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        
        ChangeState(new KeeseDeadState(this));
    }

    // Combat actor and hurtbox info
    public DamageTeam Team => DamageTeam.Enemy;

    public IEnumerable<IHurtbox> Hurtboxes
    {
        get { yield return _bodyHurtbox; }
    }

    //sprite sheet from https://www.spriters-resource.com/nes/legendofzelda/asset/31806/
    public void Draw(SpriteBatch spriteBatch)
    {
        _animationController.Draw(spriteBatch, Position, IsFacingLeft);
    }

    public virtual void Update(GameTime gameTime)
    {
        if (CurrentState is not KeeseDeadState){
            _animationController.Update(gameTime);
            Animation?.Update(gameTime);
            CurrentState.Update(gameTime);

            IsGoingUp = MoveVertical(IsGoingUp);
            IsFacingLeft = MoveHorizontal(IsFacingLeft);

            UpdatePosition();
        }
        else{
            if (deadAnimationBuffer < 8)
            {
                deadAnimationBuffer++;
                _animationController.Update(gameTime);
                Animation?.Update(gameTime);
                CurrentState.Update(gameTime);
            }
            else{
                IsDead = true;
                //play pop animation here
            }
        }
    }

    /// <summary>
    ///     Uses the velocity to update the keese's position.
    /// </summary>
    private void UpdatePosition()
    {
        Position += Velocity;
    }

    public bool MoveVertical(bool IsGoingUp)
    {
        var temp = Velocity.Y;
        IsGoingUp = CheckVerticalBoundary(IsGoingUp);
        if (temp != Velocity.Y) return IsGoingUp; //if Y velocity is changed due to bounding box, return

        var n = _rng.Next(60);
        if (n == 0) IsGoingUp = FlipVertical(IsGoingUp);
        return IsGoingUp; //if not, give chance for random flipping
    }

    public bool CheckVerticalBoundary(bool IsGoingUp)
    {
        if (Position.Y < StartingPosY - BoxContainer.Y)
        {
            IsGoingUp = false;
            Velocity = Velocity with { Y = -Velocity.Y };
        }
        else if (Position.Y > StartingPosY + BoxContainer.Y)
        {
            IsGoingUp = true;
            Velocity = Velocity with { Y = -Velocity.Y };
        }

        return IsGoingUp;
    }

    public bool FlipVertical(bool IsGoingUp)
    {
        if (IsGoingUp)
        {
            IsGoingUp = false;
            if (Velocity.Y < 0) Velocity = Velocity with { Y = -Velocity.Y };
        }
        else
        {
            IsGoingUp = true;
            if (Velocity.Y > 0) Velocity = Velocity with { Y = -Velocity.Y };
        }

        return IsGoingUp;
    }

    public bool MoveHorizontal(bool IsFacingLeft)
    {
        var temp = Velocity.X;
        IsFacingLeft = CheckHorizontalBoundary(IsFacingLeft);
        if (temp != Velocity.X) return IsFacingLeft;//if X velocity is changed due to bounding box, return

        var n = _rng.Next(60);
        if (n == 0) IsFacingLeft = FlipHorizontal(IsFacingLeft);
        return IsFacingLeft;//if not, give chance for random flipping
    }

    public bool CheckHorizontalBoundary(bool IsFacingLeft)
    {
        if (Position.X < StartingPosX - BoxContainer.X)
        {
            IsFacingLeft = false;
            Velocity = Velocity with { X = -Velocity.X };
        }
        else if (Position.X > StartingPosX + BoxContainer.X)
        {
            IsFacingLeft = true;
            Velocity = Velocity with { X = -Velocity.X };
        }

        return IsFacingLeft;
    }

    public bool FlipHorizontal(bool IsFacingLeft)
    {
        if (IsFacingLeft)
        {
            IsFacingLeft = false;
            if (Velocity.X < 0) Velocity = Velocity with { X = -Velocity.X };
        }
        else
        {
            IsFacingLeft = true;
            if (Velocity.X > 0) Velocity = Velocity with { X = -Velocity.X };
        }

        return IsFacingLeft;
    }

    public void FlipDirection()
    {
        Velocity = new Vector2(-Velocity.X, Velocity.Y);
        IsFacingLeft = !IsFacingLeft;
    }
}