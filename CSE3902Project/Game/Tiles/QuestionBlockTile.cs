using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Tiles;

public class QuestionBlockTile : Tile
{
    private readonly BlockContents _contents;
    private bool _isUsed;

    //empty one (the tile display uses this)
    public QuestionBlockTile(Vector2 position)
        : this(position, new BlockContents())
    {
    }

    public QuestionBlockTile(Vector2 position, BlockContents contents)
        : base(TileType.QuestionBlock, TileSpriteFactory.Instance.CreateQuestionBlockSprite(), position)
    {
        _contents = contents;
    }

    //pops the item out when hit from below, then becomes a used block
    public override void HitFromBelow()
    {
        if (_isUsed || !_contents.HasItem)
        {
            return;
        }

        _contents.PopOut(Position);
        _isUsed = true;
        ChangeSprite(TileSpriteFactory.Instance.CreateStepSprite());
    }
}
