using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Graphics;

/// <summary>
/// Interface for objects and entities that can be animated.
/// </summary>
public interface IAnimatable
{
    bool IsFacingLeft { get; }
    IState CurrentState { get; }
}