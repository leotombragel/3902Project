using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

/// <summary>
/// Interface for defining entity states.
/// </summary>
public interface IState
{
    /// <summary>
    /// The name of the current animation. This should match a key in a corresponding IAnimationSource.
    /// </summary>
    string AnimationName { get; }
    void Enter();
    void Update(GameTime gameTime);
    void Exit();
}