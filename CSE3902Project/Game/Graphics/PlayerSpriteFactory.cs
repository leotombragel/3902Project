using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// <summary>
/// Loads sprites for the game.
/// </summary>
public class PlayerSpriteFactory
{
    public static PlayerSpriteFactory Instance { get; } = new PlayerSpriteFactory();

    private Texture2D _idlePlayerSprite;
    private Texture2D _walkingPlayerSprite;
    private Texture2D _rockingPlayerSprite;

    private Texture2D _linkSheet;
    private Texture2D _keeseSheet;
    private Texture2D _stalfoSheet;

    public void LoadAllAssets(ContentManager content)
    {
        _idlePlayerSprite = content.Load<Texture2D>("images/idle");
        _walkingPlayerSprite = content.Load<Texture2D>("images/spritesheet");
        _rockingPlayerSprite = content.Load<Texture2D>("images/spritesheet_rocking");
        _linkSheet = content.Load<Texture2D>("images/player");
        _keeseSheet = content.Load<Texture2D>("images/DungeonEnemiesCUT2");
        _stalfoSheet = content.Load<Texture2D>("images/DungeonEnemiesCUT2");

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

    public Sprite CreateLinkSprite()
    {
        var sprite = new Sprite(_linkSheet);
        sprite.Scale = new Vector2(2.0f, 2.0f);
        return sprite;
    }

    public Sprite CreateKeeseSprite()
    {
        var sprite = new Sprite(_keeseSheet);
        sprite.Scale = new Vector2(2.0f, 2.0f);
        return sprite;
    }

    public Sprite CreateStalfoSprite()
    {
        var sprite = new Sprite(_stalfoSheet);
        sprite.Scale = new Vector2(2.0f, 2.0f);
        return sprite;
    }
}