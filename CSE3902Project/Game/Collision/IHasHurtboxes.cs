using System.Collections.Generic;

namespace CSE3902Project.Game.Collision;

/// <summary>
///     Interface for objects that have at least one hurtbox for collision detection.
/// </summary>
public interface IHasHurtboxes
{
    /// <summary>
    ///     A collection of hurtboxes for the object or entity.
    /// </summary>
    IEnumerable<IHurtbox> Hurtboxes { get; }
}