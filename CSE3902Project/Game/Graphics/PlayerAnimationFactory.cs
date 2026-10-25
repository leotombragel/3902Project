using System;
using System.Collections.Generic;

namespace CSE3902Project.Game.Graphics;

public class PlayerAnimationFactory : IAnimationFactory
{
    private static readonly Dictionary<string, Func<SpriteAnimation>> AnimationFactories = new()
    {
        ["PlayerIdle"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            24, //32
            32, //32
            1,
            0.08f,
            32 * 3,
            0,
            8),
        ["PlayerWalk"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            24,
            32,
            3,
            0.08f,
            32 * 5,
            0,
            8),
        ["PlayerJump"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            24 - 3,
            32,
            1,
            0.08f,
            32 * 5,
            32,
            8),
        ["PlayerFall"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            24,
            32,
            1,
            0.08f,
            32 * 6 - 3,
            32,
            8),
        ["PlayerStab"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            24,
            32,
            4,
            0.15f,
            0,
            32 * 3,
            8),
        ["PlayerThrow"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            32, // everything else uses 24, might need to change this
            32,
            2,
            0.15f,
            32 * 5,
            32 * 4,
            false
        ),
        ["PlayerDead"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            32,
            32,
            1,
            0.08f,
            32 * 6,
            32 * 2
        ),
        ["PlayerBow"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            32,
            32,
            2,
            0.25f,
            32 * 0,
            32 * 5
        ),
        ["PlayerBowIdle"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            24,
            32,
            1,
            0.25f,
            32 * 2,
            32 * 5
        ),
        ["PlayerBowWalk"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            32,
            32,
            3,
            0.08f,
            32 * 3,
            32 * 5
        ),
        ["PlayerBowJump"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            24,
            32,
            1,
            0.08f,
            32 * 6,
            32 * 5
        ),
        ["PlayerBowFall"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
            24,
            32,
            1,
            0.08f,
            32 * 7,
            32 * 5
        ),
        ["Arrow"] = () => new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(), 32,
            32,
            1,
            0.08f,
            32 * 7,
            32 * 4)
    };

    public ISprite Create(string animationName)
    {
        return AnimationFactories[animationName]();
    }
}