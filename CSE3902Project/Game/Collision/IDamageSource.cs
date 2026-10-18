using CSE3902Project.Game.Damage;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Collision;

public interface IDamageSource
{
    /// <summary>
    ///     The owner, or source, of the damage.
    /// </summary>
    ICombatActor Owner { get; }

    DamageInfo Damage { get; }

    Rectangle Bounds { get; }
    bool IsCollisionEnabled { get; }
    bool IsFinished { get; }

    /// <summary>
    ///     Checks if this damage source has already hit the given victim to prevent it from being damaged every frame.
    /// </summary>
    /// <param name="victim">The entity to check.</param>
    /// <returns></returns>
    bool HasHit(ICombatActor victim);

    /// <summary>
    ///     Records that this damage source has hit the given victim.
    /// </summary>
    /// <param name="victim"></param>
    void RecordHit(ICombatActor victim);

    /// <summary>
    ///     The action taken when the damage source collides with a victim.
    /// </summary>
    /// <param name="victim"></param>
    void OnHit(ICombatActor victim);
}