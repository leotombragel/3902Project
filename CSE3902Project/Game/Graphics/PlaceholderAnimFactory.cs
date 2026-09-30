using System.Collections.Generic;

namespace CSE3902Project.Game.Graphics;

/// <summary>
///     This is a basic class that just stores player animations in a dictionary until a better factory is implemented.
/// </summary>
public class PlaceholderAnimFactory : IAnimationSource
{
    private readonly Dictionary<string, SpriteAnimation> _animations;

    public PlaceholderAnimFactory()
    {
        _animations = new Dictionary<string, SpriteAnimation>
        {
            // Initialize with some default animations

            // Original Funky Kong animations
            ["FunkyWalk"] = new(SpriteFactory.Instance.CreateWalkingPlayerSprite(),
                39,
                44,
                16,
                0.08f),
            ["FunkyRock"] = new(SpriteFactory.Instance.CreateRockingPlayerSprite(),
                52,
                56,
                40,
                0.08f),
            ["FunkyIdle"] = new(SpriteFactory.Instance.CreateIdlePlayerSprite(),
                52,
                56,
                1,
                0.08f),

            // Link animations will go below
            ["PlayerIdle"] = new(SpriteFactory.Instance.CreateLinkSprite(),
                32,
                32,
                1,
                0.08f,
                32 * 3,
                0),
            ["PlayerWalk"] = new(SpriteFactory.Instance.CreateLinkSprite(),
                32,
                32,
                3,
                0.08f,
                32 * 5,
                0),
            ["PlayerJump"] = new(SpriteFactory.Instance.CreateLinkSprite(),
                32 - 3, // I think I messed up the sprite sheet here
                32,
                1,
                0.08f,
                32 * 5,
                32 * 1),
            ["PlayerFall"] = new(SpriteFactory.Instance.CreateLinkSprite(),
                32,
                32,
                1,
                0.08f,
                32 * 6 - 3, // Same with this one
                32 * 1),
            ["PlayerStab"] = new(SpriteFactory.Instance.CreateLinkSprite(),
                32,
                32,
                4,
                0.15f,
                0,
                32 * 3
            ),
            ["PlayerDead"] = new(SpriteFactory.Instance.CreateLinkSprite(),
                32,
                32,
                1,
                0.08f,
                32 * 6,
                32 * 2
            ),

            //Keese animations
            ["KeeseFlying"] = new(SpriteFactory.Instance.CreateKeeseSprite(),
                16,
                13,
                2,
                0.08f,
                184,
                13,
                1), //bufferWidth
            ["KeeseStopped"] = new(SpriteFactory.Instance.CreateKeeseSprite(),
                16,
                13,
                1,
                0.08f,
                183,
                13),
            ["KeeseDead"] = new(SpriteFactory.Instance.CreateKeeseSprite(),
                16,
                13,
                1,
                0.08f,
                183,
                30),
            //Stalfo animations
            ["StalfoLeft"] = new(SpriteFactory.Instance.CreateStalfoSprite(),
                16,
                16,
                1,
                0.08f,
                2,
                59),
            ["StalfoRight"] = new(SpriteFactory.Instance.CreateStalfoSprite(),
                16,
                16,
                1,
                0.08f,
                2,
                59),
            ["StalfoDead"] = new(SpriteFactory.Instance.CreateStalfoSprite(),
                16,
                16,
                2,
                0.08f,
                2,
                59)
        };
    }

    public ISprite Create(string animationName)
    {
        return _animations[animationName];
    }
}