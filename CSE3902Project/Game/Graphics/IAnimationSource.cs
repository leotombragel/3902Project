namespace CSE3902Project.Game.Graphics;

public interface IAnimationSource
{
    ISprite Create(string animationName);
}