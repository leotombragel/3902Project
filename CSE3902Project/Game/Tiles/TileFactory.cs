using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Tiles;

/// Creates the tile class that matches a Tile Type
public static class TileFactory
{
    public static ITile Create(TileType type, Vector2 position)
    {
        return type switch
        {
            TileType.Ground => new GroundTile(position),
            TileType.Brick => new BrickTile(position),
            TileType.QuestionBlock => new QuestionBlockTile(position),
            TileType.Pipe => new PipeTile(position),
            TileType.Step => new StepTile(position),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown tile type")
        };
    }
}
