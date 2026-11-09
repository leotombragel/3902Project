using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// Makes the small still pictures the hud uses (heart, rupee, plain box color).
public class HudSpriteFactory
{
    private const int CellSize = 16;
    private const float DrawScale = 2.0f;
    private const int HeartTileWidth = 36;
    private const int HeartTileHeight = 32;
    private const int HeartTileSpacing = 2;

    public static HudSpriteFactory Instance { get; } = new HudSpriteFactory();

    private Texture2D _pickupSheet;
    private Texture2D _heartSheet;

    //a 1x1 white pixel, stretched and tinted to make boxes
    public Texture2D Pixel { get; private set; }

    public void LoadAllAssets(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _pickupSheet = content.Load<Texture2D>("images/pickups");
        _heartSheet = content.Load<Texture2D>("images/hearts");

        Pixel = new Texture2D(graphicsDevice, 1, 1);
        Pixel.SetData(new[] { Color.White });
    }

    //full heart and empty heart from the hearts sheet (tiles are 36x32 with 2 pixels between them)
    public Sprite CreateHeart() => CreateHeartTile(tileNumber: 0);

    public Sprite CreateEmptyHeart() => CreateHeartTile(tileNumber: 2);

    //how wide a heart is on screen
    public int HeartWidth => HeartTileWidth;

    //first frame of the rupee in the pickup sheet
    public Sprite CreateRupee() => CreateIcon(column: 0, row: 0);

    //how big the rupee is on screen
    public int IconSize => (int)(CellSize * DrawScale);

    private Sprite CreateHeartTile(int tileNumber)
    {
        int x = tileNumber * (HeartTileWidth + HeartTileSpacing);
        var sprite = new Sprite(_heartSheet);
        sprite.SourceRectangle = new Rectangle(x, 0, HeartTileWidth, HeartTileHeight);
        return sprite;
    }

    private Sprite CreateIcon(int column, int row)
    {
        var sprite = new Sprite(_pickupSheet) { Scale = new Vector2(DrawScale) };
        sprite.SourceRectangle = new Rectangle(column * CellSize, row * CellSize, CellSize, CellSize);
        return sprite;
    }
}
