using System;
using System.Collections.Generic;

namespace CSE3902Project.Game.Graphics;

public class AquaementusAnimationFactory : IAnimationFactory
{
    private static readonly Dictionary<string, Func<SpriteAnimation>> AnimationFactories = new()
    {
        ["AquaementusLiving"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateAquaementusSprite(),
            24,
            32,
            4,
            0.08f,
            1,
            11,
            1),
        ["AquaementusDead"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateAquaementusSprite(),
            24,
            32,
            4,
            0.01f,
            261,
            227,
            7),
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