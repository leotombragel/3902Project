using CSE3902Project.Game.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// Creates the sprites for items. Boomerang, bow, arrow and bomb use the Link items sheet (SpriteFactory has to be
/// loaded first). Rupee, triforce, key and heart use the pickup sheet (16x16 cells, one row each).
public class ItemSpriteFactory
{
    private const int CellSize = 16;
    private const float DrawScale = 2.0f;

    private Texture2D _pickupSheet;

    public static ItemSpriteFactory Instance { get; } = new();

    public void LoadAllAssets(ContentManager content)
    {
        _pickupSheet = content.Load<Texture2D>("images/pickups");
    }

    //link items sheet

    public ItemArt CreateTorchArt()
    {
        var animation = new SpriteAnimation(SpriteFactory.Instance.CreateTorchSprite(), 12, 30, 6, 0.2f);
        return new ItemArt(animation, new Point(12 * 2, 30 * 2));
    }

    public ItemArt CreateBoomerangArt()
    {
        // same frames as the thrown boomerang
        var animation = new SpriteAnimation(SpriteFactory.Instance.CreateItemsSheet(), 12, 12, 4, 0.1f, 126, 85, 12);
        return new ItemArt(animation, new Point(12 * 2, 12 * 2));
    }

    public ItemArt CreateBowArt()
    {
        return CreateLinkSheetArt(new Rectangle(32, 156, 8, 16));
    }

    public ItemArt CreateArrowArt()
    {
        return CreateLinkSheetArt(new Rectangle(32, 132, 7, 16));
    }

    public ItemArt CreateBombArt()
    {
        return CreateLinkSheetArt(new Rectangle(102, 108, 14, 16));
    }

    //pickup sheet

    public ItemArt CreateRupeeArt()
    {
        return CreatePickupArt(4, 0.15f, 0, 0);
    }

    public ItemArt CreateTriforceArt()
    {
        return CreatePickupArt(4, 0.2f, 0, 1);
    }

    public ItemArt CreateKeyArt()
    {
        return CreatePickupArt(2, 0.4f, 0, 2);
    }

    public ItemArt CreateHeartArt()
    {
        return CreatePickupArt(2, 0.35f, 2, 2);
    }

    private static ItemArt CreateLinkSheetArt(Rectangle source)
    {
        var sprite = SpriteFactory.Instance.CreateItemsSheet();
        sprite.SourceRectangle = source;
        return new ItemArt(sprite, new Point((int)(source.Width * DrawScale), (int)(source.Height * DrawScale)));
    }

    private ItemArt CreatePickupArt(int frameCount, float frameDuration, int column, int row)
    {
        var sprite = new Sprite(_pickupSheet) { Scale = new Vector2(DrawScale) };
        var animation = new SpriteAnimation(sprite, CellSize, CellSize, frameCount, frameDuration,
            column * CellSize, row * CellSize);
        return new ItemArt(animation, new Point((int)(CellSize * DrawScale), (int)(CellSize * DrawScale)));
    }
}