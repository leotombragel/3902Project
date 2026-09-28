using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Tiles;

/// A row of ground tiles that runs across the bottom of the screen.
public class GroundRow
{
    public IReadOnlyList<ITile> Tiles { get; }

    public GroundRow(int screenWidth, int screenHeight)
    {
        var tiles = new List<ITile>();
        var y = screenHeight - Tile.Size;

        for (var x = 0; x < screenWidth; x += Tile.Size)
        {
            tiles.Add(new GroundTile(new Vector2(x, y)));
        }

        Tiles = tiles;
    }

    public void Update(GameTime gameTime)
    {
        foreach (var tile in Tiles)
        {
            tile.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var tile in Tiles)
        {
            tile.Draw(spriteBatch);
        }
    }
}
