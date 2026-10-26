using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Tiles;

public class BrickTile : Tile
{
    private readonly BlockContents _contents;
    private bool _isBroken;

    //plain brick with nothing inside
    public BrickTile(Vector2 position)
        : this(position, new BlockContents())
    {
    }

    public BrickTile(Vector2 position, BlockContents contents)
        : base(TileType.Brick, TileSpriteFactory.Instance.CreateBrickSprite(), position)
    {
        _contents = contents;
    }

    public override bool IsBroken
    {
        get { return _isBroken; }
    }

    //breaks when hit from below, anything inside pops out
    public override void HitFromBelow()
    {
        _contents.PopOut(Position);
        _isBroken = true;
    }
}
