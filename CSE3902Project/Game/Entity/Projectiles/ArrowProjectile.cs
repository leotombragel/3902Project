using System;
using System.Collections.Generic;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Entity.Projectiles;

public class ArrowProjectile : IProjectile
{
    private const int AnimationFrameWidth = 32;
    private const int AnimationFrameHeight = 32;
    private const float Speed = 5.0f;
    private const float Gravity = 0.07f;
    private const int CollisionWidth = 24;
    private const int CollisionHeight = 8;
    private const int OffsetX = 0;
    private const int OffsetY = 40;
    private const float OriginX = 16.0f;

    /// <summary>
    ///     This is the row of pixels the shaft of the arrow sits on in its 32x32 section of the player sprite sheet. Putting
    ///     the origin here makes the arrow rotate around its true center.
    /// </summary>
    private const float OriginY = 22.0f;

    private readonly HashSet<ICombatActor> _hitActors = new();

    public ArrowProjectile(
        int startingX,
        int startingY,
        bool isFacingLeft,
        ICombatActor owner)
    {
        Owner = owner;
        Damage = new DamageInfo(this, 1, Vector2.Zero, DamageTeam.Player);
        IsFacingLeft = isFacingLeft;

        Position = new Vector2(startingX + OffsetX, startingY + OffsetY);
        Velocity = new Vector2(isFacingLeft ? -Speed : Speed, 0.0f);

        var playerAnimationFactory = new PlayerAnimationFactory();
        Animation = (SpriteAnimation)playerAnimationFactory.Create("Arrow");
        Animation.Sprite.Origin = new Vector2(
            AnimationFrameWidth / 2.0f,
            AnimationFrameHeight / 2.0f);

        IsCollisionEnabled = true;
        IsFinished = false;
    }

    public Vector2 Position { get; set; }
    public bool IsFacingLeft { get; set; }
    public Vector2 Velocity { get; private set; }

    public ICombatActor Owner { get; }
    public DamageInfo Damage { get; }

    public Rectangle Bounds =>
        new(
            (int)(Position.X - CollisionWidth / 2.0f),
            (int)(Position.Y - CollisionHeight / 2.0f),
            CollisionWidth,
            CollisionHeight);

    public bool IsCollisionEnabled { get; private set; }
    public bool IsFinished { get; private set; }

    public bool HasHit(ICombatActor victim)
    {
        return _hitActors.Contains(victim);
    }

    public void RecordHit(ICombatActor victim)
    {
        _hitActors.Add(victim);
    }

    public void OnHit(ICombatActor victim)
    {
        // Remove arrow for now
        IsFinished = true;
        IsCollisionEnabled = false;
    }
    
    public void OnHitTile()
    {
        IsFinished = true;
        IsCollisionEnabled = false;
    }

    public SpriteAnimation Animation { get; set; }

    public void Draw(SpriteBatch s)
    {
        Animation?.Draw(s, Position);
    }

    public void Update(GameTime gameTime)
    {
        Animation?.Update(gameTime);

        Velocity = new Vector2(
            Velocity.X,
            Velocity.Y + Gravity);
        Position += Velocity;

        if (Animation is null) return;
        
        // The arrow points towards its velocity vector, which makes its trajectory look more realistic.
        Animation.Sprite.Rotation = MathF.Atan2(Velocity.Y, Velocity.X);
        Animation.Sprite.Origin = new Vector2(OriginX, OriginY);
    }
}