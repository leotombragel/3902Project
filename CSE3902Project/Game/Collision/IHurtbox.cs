using CSE3902Project.Game.Damage;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Collision;

/// <summary>
///     Interface for objects that store hurtbox information for collision detection.
/// </summary>
public interface IHurtbox
{
    Rectangle Bounds { get; }
    ICombatActor Owner { get; }
    bool IsEnabled { get; }
}