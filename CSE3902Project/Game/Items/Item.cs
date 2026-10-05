using System;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Items;

/// Shared behavior for all items. Items bob up and down while their sprite animates.
public abstract class Item : IItem
{
    private const float BobHeight = 3.0f; // pixels
    private const float BobPeriod = 1.6f; // seconds

    private readonly ISprite _sprite;
    private readonly Point _size;
    private readonly Vector2 _restingPosition;
    private float _elapsedSeconds;

    public ItemType Type { get; }
    public Vector2 Position { get; private set; }
    public Rectangle Bounds => new((int)Position.X, (int)Position.Y, _size.X, _size.Y);

    protected Item(ItemType type, ItemArt art, Vector2 center)
    {
        Type = type;
        _sprite = art.Sprite;
        _size = art.Size;
        _restingPosition = center - new Vector2(art.Size.X / 2f, art.Size.Y / 2f);
        Position = _restingPosition;
    }

    public virtual void Update(GameTime gameTime)
    {
        _sprite.Update(gameTime);

        _elapsedSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
        var bob = MathF.Sin(_elapsedSeconds / BobPeriod * MathHelper.TwoPi) * BobHeight;
        Position = _restingPosition + new Vector2(0, bob);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        // Round to whole pixels so the sprite stays sharp
        _sprite.Draw(spriteBatch, new Vector2(MathF.Round(Position.X), MathF.Round(Position.Y)));
    }
}
