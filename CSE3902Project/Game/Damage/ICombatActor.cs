using CSE3902Project.Game.Collision;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Damage;

/// <summary>
///     Interface for entities that can deal and receive damage.
/// </summary>
public interface ICombatActor : IMortal, IHasHurtboxes
{
    /// <summary>
    ///     The team that this combat actor belongs to, for preventing friendly fire.
    /// </summary>
    DamageTeam Team { get; }

    Vector2 Position { get; }
    bool IsFacingLeft { get; }

    void FlipDirection();
}