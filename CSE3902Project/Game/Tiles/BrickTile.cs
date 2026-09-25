using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Tiles;

public class BrickTile : Tile
{
    public BrickTile(Vector2 position)
        : base(TileType.Brick, TileSpriteFactory.Instance.CreateBrickSprite(), position)
    {
    }
}
