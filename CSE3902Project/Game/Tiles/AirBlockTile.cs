using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Tiles;

public class AirBlockTile : Tile
{
    public AirBlockTile(Vector2 position)
        : base(TileType.AirBlock, TileSpriteFactory.Instance.CreateAirBlockSprite(), position)
    {
    }
}
