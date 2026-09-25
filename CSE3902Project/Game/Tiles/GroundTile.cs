using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Tiles;

public class GroundTile : Tile
{
    public GroundTile(Vector2 position)
        : base(TileType.Ground, TileSpriteFactory.Instance.CreateGroundSprite(), position)
    {
    }
}
