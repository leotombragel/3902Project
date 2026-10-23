using System;
using System.Collections.Generic;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Entity.Projectiles;

public class ArrowProjectile : IProjectile
{
    public ArrowProjectile(int startingX,
        int startingY,
        bool isFacingLeft,
        ICombatActor owner)
    {
        Position = new Vector2(startingX, startingY);
        IsFacingLeft = isFacingLeft;
        Owner = owner;
        Damage = new DamageInfo(owner, 1, Vector2.Zero, DamageTeam.Player);
        var playerAnimFactory = new PlayerAnimationFactory();
        Animation = (SpriteAnimation)playerAnimFactory.Create("Arrow");
        Bounds = new Rectangle(startingX + 20, startingY + 38, 23, 10);
        IsCollisionEnabled = true;
        IsFinished = false;
    }

    public Vector2 Position { get; set; }
    public bool IsFacingLeft { get; set; }

    public ICombatActor Owner { get; }
    public DamageInfo Damage { get; }
    public Rectangle Bounds { get; }
    public bool IsCollisionEnabled { get; private set; }
    public bool IsFinished { get; private set; }

    private readonly HashSet<ICombatActor> _hitActors = new();
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

    public SpriteAnimation Animation { get; set; }

    public Vector2 Velocity { get; set; }

    public void Draw(SpriteBatch s)
    {
        Animation?.Draw(s, Position, IsFacingLeft);
    }

    public void Update(GameTime gameTime)
    {
    }
}