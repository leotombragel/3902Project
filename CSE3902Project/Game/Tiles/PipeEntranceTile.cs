using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Tiles;

/// A non-solid map marker containing the configuration for a pipe entrance.
public sealed class PipeEntranceTile : ITile
{
    private const int HitboxDepth = Tile.Size / 4;
    private const int HitboxProtrusion = 4;

    public PipeEntranceTile(Vector2 position, PipeSignature signature)
    {
        Position = position;
        Signature = signature;
    }

    public TileType Type => TileType.Pipe;
    public Vector2 Position { get; }
    public Rectangle Bounds => new(
        (int)Position.X,
        (int)Position.Y,
        Signature.Width * Tile.Size,
        Signature.Height * Tile.Size);
    public PipeSignature Signature { get; }

    public Rectangle InteractionBounds
    {
        get
        {
            var x = (int)Position.X;
            var y = (int)Position.Y;
            var width = Signature.Width * Tile.Size;
            var height = Signature.Height * Tile.Size;

            return Signature.EntranceDirection switch
            {
                PipeDirection.Up => new Rectangle(x, y - HitboxProtrusion, width, HitboxDepth),
                PipeDirection.Down => new Rectangle(
                    x, y + height - HitboxDepth + HitboxProtrusion, width, HitboxDepth),
                PipeDirection.Left => new Rectangle(x - HitboxProtrusion, y, HitboxDepth, height),
                PipeDirection.Right => new Rectangle(
                    x + width - HitboxDepth + HitboxProtrusion, y, HitboxDepth, height),
                _ => throw new ArgumentOutOfRangeException(nameof(Signature.EntranceDirection))
            };
        }
    }

    public bool IsBroken => false;

    public void HitFromBelow()
    {
    }

    public void Update(GameTime gameTime)
    {
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }
}
