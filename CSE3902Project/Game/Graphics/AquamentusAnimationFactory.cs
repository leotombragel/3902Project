using System;
using System.Collections.Generic;

namespace CSE3902Project.Game.Graphics;

public class AquamentusAnimationFactory : IAnimationFactory
{
    private static readonly Dictionary<string, Func<SpriteAnimation>> AnimationFactories = new()
    {
        ["AquamentusLeft"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateStalfoSprite(),
            16,
            16,
            1,
            0.08f,
            2,
            59),
        ["AquamentusRight"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateStalfoSprite(),
            16,
            16,
            1,
            0.08f,
            2,
            59),
        ["AquamentusDead"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateStalfoSprite(),
            16,
            16,
            2,
            0.08f,
            2,
            59),
        ["AquamentusAttack"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateStalfoSprite(),
            16,
            16,
            2,
            0.08f,
            2,
            59),
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