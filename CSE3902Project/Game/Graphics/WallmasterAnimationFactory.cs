using System;
using System.Collections.Generic;

namespace CSE3902Project.Game.Graphics;

public class WallmasterAnimationFactory : IAnimationFactory
{
    private static readonly Dictionary<string, Func<SpriteAnimation>> AnimationFactories = new()
    {
        ["WallmasterWaitingState"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateWallmasterSprite(),
            14,
            16,
            1,
            0.08f,
            393,
            11,
            2),
        ["WallmasterRecoverState"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateWallmasterSprite(),
            14,
            16,
            1,
            0.08f,
            393,
            11,
            2),
        ["WallmasterTrackingState"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateWallmasterSprite(),
            14,
            16,
            1,
            0.08f,
            393,
            11,
            2),
        ["WallmasterMovingDownState"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateWallmasterSprite(),
            14,
            16,
            2,
            0.2f,
            393,
            11,
            2),
        ["WallmasterMovingUpState"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateWallmasterSprite(),
            14,
            16,
            2,
            0.2f,
            393,
            11,
            2),
        ["WallmasterDeadState"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateWallmasterSprite(),
            14,
            16,
            2,
            0.08f,
            423,
            194,
            2),
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