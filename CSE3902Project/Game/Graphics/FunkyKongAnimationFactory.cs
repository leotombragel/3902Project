using System;
using System.Collections.Generic;

namespace CSE3902Project.Game.Graphics;

public class FunkyKongAnimationFactory : IAnimationFactory
{
    private static readonly Dictionary<string, Func<SpriteAnimation>> AnimationFactories = new()
    {
        ["FunkyWalk"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateWalkingPlayerSprite(),
            39,
            44,
            16,
            0.08f),
        ["FunkyRock"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateRockingPlayerSprite(),
            52,
            56,
            40,
            0.08f),
        ["FunkyIdle"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateIdlePlayerSprite(),
            52,
            56,
            1,
            0.08f),
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