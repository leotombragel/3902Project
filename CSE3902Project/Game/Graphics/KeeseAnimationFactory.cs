using System;
using System.Collections.Generic;

namespace CSE3902Project.Game.Graphics;

public class KeeseAnimationFactory : IAnimationFactory
{
    private static readonly Dictionary<string, Func<SpriteAnimation>> AnimationFactories = new()
    {
        ["KeeseFlying"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateKeeseSprite(),
            16,
            13,
            2,
            0.08f,
            184,
            13,
            1),
        ["KeeseStopped"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateKeeseSprite(),
            16,
            13,
            1,
            0.08f,
            183,
            13),
        ["KeeseDead"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateKeeseSprite(),
            16,
            13,
            1,
            0.08f,
            183,
            30),
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