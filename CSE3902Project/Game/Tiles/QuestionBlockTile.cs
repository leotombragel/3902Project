using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Tiles;

public class QuestionBlockTile : Tile
{
    public QuestionBlockTile(Vector2 position)
        : base(TileType.QuestionBlock, TileSpriteFactory.Instance.CreateQuestionBlockSprite(), position)
    {
    }
}
