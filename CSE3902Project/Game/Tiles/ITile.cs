using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Tiles;

/// A stationary block in the level (ground, bricks, pipes, etc.).
public interface ITile
{
    TileType Type { get; }
    Vector2 Position { get; }

    /// The area the tile occupies in world space. Intended for collision detection.
    Rectangle Bounds { get; }

    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
