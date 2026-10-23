using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// Creates the sprites for tiles from the tile sheet (16x16 cells: ground, brick, step, pipe on row 0,
/// question block animation on row 1, pipe body on row 2). Each cell has a 1px border around it.
public class TileSpriteFactory
{
    private const int CellSize = 16;
    private const int Border = 1; // extra pixels around each cell in the sheet
    private const int CellStride = CellSize + 2 * Border;
    private const float DrawScale = 2.0f;
    private const int QuestionFrames = 4;
    private const float QuestionFrameDuration = 0.2f;

    public static TileSpriteFactory Instance { get; } = new TileSpriteFactory();

    private Texture2D _tileSheet;
    private Texture2D _marioTileSheet;

    public void LoadAllAssets(ContentManager content)
    {
        _tileSheet = content.Load<Texture2D>("images/tiles");
        _marioTileSheet = content.Load<Texture2D>("images/MarioTiles");
    }

    public ISprite CreateGroundSprite() => CreateStaticSprite(0, 0);

    public ISprite CreateBrickSprite() => CreateStaticSprite(1, 0);

    public ISprite CreateStepSprite() => CreateStaticSprite(2, 0);

    public ISprite CreatePipeSprite() => CreateStaticSprite(3, 0);

    public ISprite CreatePipeBodySprite() => CreateStaticSprite(0, 2);

    public ISprite CreatePipeLeftBodySprite() => CreateMarioTileSprite(120, 86);

    public ISprite CreatePipeRightBodySprite() => CreateMarioTileSprite(137, 86);

    public ISprite CreatePipeLeftOpeningSprite() => CreateMarioTileSprite(120, 69);

    public ISprite CreatePipeRightOpeningSprite() => CreateMarioTileSprite(137, 69);

    public ISprite CreateQuestionBlockSprite()
    {
        return new SpriteAnimation(CreateSheetSprite(), CellSize, CellSize, QuestionFrames,
            QuestionFrameDuration, Border, CellStride + Border, 2 * Border);
    }

    private ISprite CreateStaticSprite(int column, int row)
    {
        var sprite = CreateSheetSprite();
        sprite.SourceRectangle = new Rectangle(column * CellStride + Border, row * CellStride + Border, CellSize,
            CellSize);
        return sprite;
    }

    private Sprite CreateSheetSprite()
    {
        return new Sprite(_tileSheet) { Scale = new Vector2(DrawScale) };
    }

    private Sprite CreateMarioTileSprite(int x, int y)
    {
        return new Sprite(_marioTileSheet)
        {
            Scale = new Vector2(DrawScale),
            SourceRectangle = new Rectangle(x, y, CellSize, CellSize)
        };
    }
}
