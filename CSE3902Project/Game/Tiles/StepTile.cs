using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Tiles;

public class StepTile : Tile
{
    public StepTile(Vector2 position)
        : base(TileType.Step, TileSpriteFactory.Instance.CreateStepSprite(), position)
    {
    }
}
