using System;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using CSE3902Project.Game.Damage;

namespace CSE3902Project.Game.Entity;

public class WizzrobeProjectile : IProjectile
{
    private readonly HashSet<ICombatActor> _hitTargets = new();
    private readonly ICombatActor _owner;
    private const int ThrowSpeed = 2;
    private const int MaxOutboundDistance = 3000;

    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    public bool IsFacingLeft { get; private set; }
    public ICombatActor Owner => _owner!;
    public DamageInfo Damage { get; }
    public Rectangle Bounds => new((int)XPosition, (int)YPosition, 8, 10);
    public bool IsCollisionEnabled => !IsFinished;
    public bool IsFinished{  get; private set; }
    public float XPosition { get; private set; }
    public float XDisplacement {get; private set; }
    public float YPosition { get; private set; }
    public Vector2 Velocity { get; private set; }

    public bool HasHit(ICombatActor victim)
    {
        return _hitTargets.Contains(victim);
    }

    public void RecordHit(ICombatActor victim)
    {
        _hitTargets.Add(victim);
    }

    public void OnHit(ICombatActor victim)
    {
        IsFinished = true;
    }
    
    public void OnHitTile()
    {
        IsFinished = true;
    }

    public WizzrobeProjectile(
        float startingX,
        float startingY,
        bool isFacingLeft,
        ICombatActor owner)
    {
        XPosition = isFacingLeft ? startingX : startingX + 32;
        YPosition = startingY + 16;
        IsFacingLeft = isFacingLeft;
        Velocity = new Vector2(isFacingLeft ? -ThrowSpeed : ThrowSpeed, 0.0f);
        _owner = owner;
        Damage = new DamageInfo(this, 1, Vector2.Zero, DamageTeam.Enemy);

        Sprite = SpriteFactory.Instance.CreateWizzrobeProjectileSprite();
        Animation = new SpriteAnimation(
            Sprite,
            8,
            10,
            2,
            0.12f,
            231,
            62,
            1);
    }

    private void MoveOutBound()
    {

        var distanceTravelled = Math.Abs(XDisplacement);
        var distanceToMove = Math.Min((int)Math.Abs(Velocity.X), MaxOutboundDistance - distanceTravelled);
        var direction = Math.Sign(Velocity.X);

        XPosition += direction * distanceToMove;
        XDisplacement += direction * distanceToMove;

    }

    public void Update(GameTime gameTime)
    {
        Animation?.Update(gameTime);
        MoveOutBound();

        if (Math.Abs(XDisplacement) > 400)
        {
            IsFinished = true;
        }
    }
    
    public void Draw(SpriteBatch s)
    {
        Animation?.Draw(s, new Vector2(XPosition, YPosition), !IsFacingLeft);
    }

    
}