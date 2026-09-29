using System;
using System.Collections.Generic;

namespace CSE3902Project.Game.Graphics;

public class PlayerAnimationFactory : IAnimationFactory
{
    private static readonly Dictionary<string, Func<SpriteAnimation>> AnimationFactories = new()
    {
        ["PlayerIdle"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            32,
            32,
            1,
            0.08f,
            32 * 3,
            0),
        ["PlayerWalk"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            32,
            32,
            3,
            0.08f,
            32 * 5,
            0),
        ["PlayerJump"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            32 - 3,
            32,
            1,
            0.08f,
            32 * 5,
            32),
        ["PlayerFall"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            32,
            32,
            1,
            0.08f,
            32 * 6 - 3,
            32),
        ["PlayerStab"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            32,
            32,
            4,
            0.15f,
            0,
            32 * 3
            ),
    };

    private readonly Dictionary<string, SpriteAnimation> _animations = new();

    public ISprite Create(string animationName)
    {
        if (_animations.TryGetValue(animationName, out var animation))
        {
            return animation;
        }

        animation = AnimationFactories[animationName]();
        _animations.Add(animationName, animation);
        return animation;
    }
}