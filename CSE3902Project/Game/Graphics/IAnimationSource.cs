namespace CSE3902Project.Game.Graphics;

/// <summary>
/// Interface for objects that provide a dictionary of animations.
/// </summary>
public interface IAnimationSource
{
    /// <summary>
    /// Returns the animation corresponding to the given animation key.
    /// </summary>
    /// <param name="animationName">The name of the animation to retrieve.</param>
    /// <returns></returns>
    ISprite Create(string animationName);
}