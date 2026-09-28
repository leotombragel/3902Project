using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// <summary>
/// Creates the sprites for tiles from the tile sheet (16x16 cells: ground, brick, step, pipe on row 0,
/// question block animation on row 1).
/// </summary>
public class TileSpriteFactory
{
    private const int CellSize = 16;
    private const float DrawScale = 4.0f;
    private const int QuestionFrames = 4;
    private const float QuestionFrameDuration = 0.2f;

    public static TileSpriteFactory Instance { get; } = new TileSpriteFactory();

    private Texture2D _tileSheet;

    public void LoadAllAssets(ContentManager content)
    {
        _tileSheet = content.Load<Texture2D>("images/tiles");
    }

    public ISprite CreateGroundSprite() => CreateStaticSprite(0, 0);

    public ISprite CreateBrickSprite() => CreateStaticSprite(1, 0);

    public ISprite CreateStepSprite() => CreateStaticSprite(2, 0);

    public ISprite CreatePipeSprite() => CreateStaticSprite(3, 0);

    public ISprite CreateQuestionBlockSprite()
    {
        return new SpriteAnimation(CreateSheetSprite(), CellSize, CellSize, QuestionFrames,
            QuestionFrameDuration, 0, CellSize);
    }

    private ISprite CreateStaticSprite(int column, int row)
    {
        var sprite = CreateSheetSprite();
        sprite.SourceRectangle = new Rectangle(column * CellSize, row * CellSize, CellSize, CellSize);
        return sprite;
    }

    private Sprite CreateSheetSprite()
    {
        return new Sprite(_tileSheet) { Scale = new Vector2(DrawScale) };
    }
}
