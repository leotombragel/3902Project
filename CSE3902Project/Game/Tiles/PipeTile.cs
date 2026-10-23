using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Tiles;

public class PipeTile : Tile
{
    public PipeTile(Vector2 position)
        : base(TileType.Pipe, TileSpriteFactory.Instance.CreatePipeSprite(), position)
    {
    }

    public PipeTile(Vector2 position, ISprite sprite)
        : base(TileType.Pipe, sprite, position)
    {
    }
}
