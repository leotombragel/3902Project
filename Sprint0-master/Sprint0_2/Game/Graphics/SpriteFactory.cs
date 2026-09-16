using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0_2.Game.Graphics;

/// <summary>
/// Loads sprites for the game.
/// </summary>
public class SpriteFactory
{
    public static SpriteFactory Instance { get; } = new SpriteFactory();

    private Texture2D _idlePlayerSprite;
    private Texture2D _walkingPlayerSprite;
    private Texture2D _rockingPlayerSprite;

    public void LoadAllAssets(ContentManager content)
    {
        _idlePlayerSprite = content.Load<Texture2D>("images/idle");
        _walkingPlayerSprite = content.Load<Texture2D>("images/spritesheet");
        _rockingPlayerSprite = content.Load<Texture2D>("images/spritesheet_rocking");
    }
    
    public Sprite CreateIdlePlayerSprite()
    {
        return new Sprite(_idlePlayerSprite);
    }

    public Sprite CreateWalkingPlayerSprite()
    {
        return new Sprite(_walkingPlayerSprite);
    }

    public Sprite CreateRockingPlayerSprite()
    {
        return new Sprite(_rockingPlayerSprite);
    }
}