using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Tiles;

/// Shared behavior for all tiles. A tile only stores where it is and delegates how it looks to its sprite.
public abstract class Tile : ITile
{
    /// Width and height of a tile in pixels on screen (16px art scaled 4x, matching Link's 64px frame).
    public const int Size = 64;

    private readonly ISprite _sprite;

    public TileType Type { get; }
    public Vector2 Position { get; }
    public Rectangle Bounds => new((int)Position.X, (int)Position.Y, Size, Size);

    protected Tile(TileType type, ISprite sprite, Vector2 position)
    {
        Type = type;
        _sprite = sprite;
        Position = position;
    }

    public virtual void Update(GameTime gameTime)
    {
        _sprite.Update(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, Position);
    }
}
