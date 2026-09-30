using System;
using System.Collections.Generic;

namespace CSE3902Project.Game.Graphics;

public class WizzrobeAnimationFactory : IAnimationFactory
{
    private static readonly Dictionary<string, Func<SpriteAnimation>> AnimationFactories = new()
    {
        ["WizzrobeVisible"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateWizzrobeSprite(),
            14,
            16,
            2,
            0.08f,
            127,
            90,
            3),
        ["WizzrobeInvisible"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateWizzrobeSprite(),
            5,
            5,
            1,
            0.08f,
            107,
            94),
        ["WizzrobeDead"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateWizzrobeSprite(),
            15,
            19,
            1,
            0.08f,
            126,
            107),
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