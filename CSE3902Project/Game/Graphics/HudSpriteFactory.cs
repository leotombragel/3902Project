using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// Makes the small still pictures the hud uses (heart, rupee, plain box color). Uses the pickup sheet.
public class HudSpriteFactory
{
    private const int CellSize = 16;
    private const float DrawScale = 2.0f;

    public static HudSpriteFactory Instance { get; } = new HudSpriteFactory();

    private Texture2D _pickupSheet;

    //a 1x1 white pixel, stretched and tinted to make boxes
    public Texture2D Pixel { get; private set; }

    public void LoadAllAssets(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _pickupSheet = content.Load<Texture2D>("images/pickups");

        Pixel = new Texture2D(graphicsDevice, 1, 1);
        Pixel.SetData(new[] { Color.White });
    }

    //first frame of the heart in the pickup sheet
    public Sprite CreateHeart() => CreateIcon(column: 2, row: 2);

    //first frame of the rupee in the pickup sheet
    public Sprite CreateRupee() => CreateIcon(column: 0, row: 0);

    //how big the icons are on screen
    public int IconSize => (int)(CellSize * DrawScale);

    private Sprite CreateIcon(int column, int row)
    {
        var sprite = new Sprite(_pickupSheet) { Scale = new Vector2(DrawScale) };
        sprite.SourceRectangle = new Rectangle(column * CellSize, row * CellSize, CellSize, CellSize);
        return sprite;
    }
}
