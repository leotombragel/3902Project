using System;
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
    }

    public ICombatActor Owner { get; }
    public DamageInfo Damage { get; }
    public Rectangle Bounds { get; }
    public bool IsCollisionEnabled { get; }
    public bool IsFinished { get; }

    public bool HasHit(ICombatActor victim)
    {
        throw new NotImplementedException();
    }

    public void RecordHit(ICombatActor victim)
    {
        throw new NotImplementedException();
    }

    public void OnHit(ICombatActor victim)
    {
        throw new NotImplementedException();
    }

    public SpriteAnimation Animation { get; set; }
    public Vector2 Velocity { get; }

    public void Draw(SpriteBatch s)
    {
    }

    public void Update(GameTime gameTime)
    {
    }
}