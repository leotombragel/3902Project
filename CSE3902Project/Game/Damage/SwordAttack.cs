using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Damage;

public class SwordAttack : IAttack
{
    private readonly HashSet<ICombatActor> _hitTargets = new();
    private float _elapsedSeconds;

    public SwordAttack(ICombatActor owner)
    {
        Owner = owner;
        Damage = new DamageInfo(this, 1, Vector2.Zero, DamageTeam.Player);
    }

    public ICombatActor Owner { get; }
    public DamageInfo Damage { get; }
    public bool IsFinished => _elapsedSeconds >= 0.32f;

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
    }
    
    public void OnHitTile()
    {
    }

    public void Update(GameTime gameTime)
    {
        _elapsedSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public bool IsCollisionEnabled => _elapsedSeconds <= 0.24f;

    public Rectangle Bounds
    {
        get
        {
            var x = Owner.IsFacingLeft
                ? (int)Owner.Position.X - 3
                : (int)Owner.Position.X + 26;

            return new Rectangle(
                x,
                (int)Owner.Position.Y + 40,
                24,
                16);
        }
    }
}