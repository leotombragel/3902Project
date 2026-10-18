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
///     keese enemy class, implements IMortal
/// </summary>
public class KeeseEnemy : StatefulEntityBase, IEnemy
{
    private const float VerticalMoveSpeed = 4.0f;
    private const float MaxSpeed = 3.0f;
    private const int StartingPosX = 400; //figure out how to set these through constructor late

    //movement
    private static readonly Random _rng = new();

    //animation
    private readonly AnimationController _animationController;
    private readonly IHurtbox _bodyHurtbox;

    private readonly int StartingPosY = 100;

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
        Velocity = Vector2.Zero;
        CurrentState = new KeeseFlyingState(this);

        // TODO Adjust the hurtbox data based on actual sprite
        _bodyHurtbox = new BodyHurtbox(this, new Point(4, 4), new Point(20, 20));
    }

    public Sprite Sprite { get; set; }
    public bool IsGoingUp { get; private set; }
    public Vector2 Velocity { get; private set; }
    public Vector2 BoxContainer { get; } //constricting values so keese doesn't fly off the screen
    public SpriteAnimation Animation { get; set; }

    //state
    public bool IsFacingLeft { get; private set; }
    public bool IsDead { get; set; }

    public void TakeDamage(DamageInfo damageInfo)
    {
        // TODO
        Console.WriteLine("Keese took damage.");
    }

    public Vector2 Position { get; private set; }

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
        _animationController.Update(gameTime);
        Animation?.Update(gameTime);
        CurrentState.Update(gameTime);

        IsGoingUp = MoveVertical(IsGoingUp);
        IsFacingLeft = MoveHorizontal(IsFacingLeft);

        UpdatePosition();
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
        if (temp != Velocity.Y) return IsGoingUp;

        var n = _rng.Next(60);
        if (n == 0) IsGoingUp = FlipVertical(IsGoingUp);
        return IsGoingUp;
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
        if (temp != Velocity.X) return IsFacingLeft;

        var n = _rng.Next(60);
        if (n == 0) IsFacingLeft = FlipHorizontal(IsFacingLeft);
        return IsFacingLeft;
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
}