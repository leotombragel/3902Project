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

    /// true once a brick got hit and broke, the stage removes it
    bool IsBroken { get; }

    /// called when the player hits the tile from below
    void HitFromBelow();

    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
