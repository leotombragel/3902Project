using System.Collections.Generic;

namespace CSE3902Project.Game.Graphics;

/// <summary>
/// This is a basic class that just stores player animations in a dictionary until a better factory is implemented.
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
            ["FunkyWalk"] = new SpriteAnimation(SpriteFactory.Instance.CreateWalkingPlayerSprite(),
                39,
                44,
                16,
                0.08f),
            ["FunkyRock"] = new SpriteAnimation(SpriteFactory.Instance.CreateRockingPlayerSprite(),
                52,
                56,
                40,
                0.08f),
            ["FunkyIdle"] = new SpriteAnimation(SpriteFactory.Instance.CreateIdlePlayerSprite(),
                52,
                56,
                1,
                0.08f),
            
            // Link animations will go below
            ["PlayerIdle"] = new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
                32,
                32,
                1,
                0.08f,
                32 * 3,
                0),
            ["PlayerWalk"] = new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
                32,
                32,
                3,
                0.08f,
                32 * 5,
                0),
            ["PlayerJump"] = new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
                32 - 3, // I think I messed up the sprite sheet here
                32,
                1,
                0.08f,
                32 * 5,
                32 * 1),
            ["PlayerFall"] = new SpriteAnimation(SpriteFactory.Instance.CreateLinkSprite(),
                32,
                32,
                1,
                0.08f,
                32 * 6 - 3, // Same with this one
                32 * 1),
        };
    }

    public ISprite Create(string animationName)
    {
        return _animations[animationName];
    }
}