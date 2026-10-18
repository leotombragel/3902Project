using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Damage;

/// <summary>
/// Interface for objects and entities that can be animated.
/// </summary>
public interface IMortal
{
    bool IsDead { get; set;  }
    void TakeDamage(DamageInfo damageInfo);
}