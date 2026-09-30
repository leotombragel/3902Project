namespace CSE3902Project.Game.Graphics;

/// <summary>
/// Creates animations by name.
/// </summary>
public interface IAnimationFactory
{
    ISprite Create(string animationName);
}