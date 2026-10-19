using System;
using System.Collections.Generic;
using CSE3902Project.Game.Collision;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Items;

/// Shared behavior for all items. Items bob up and down while their sprite animates.
public abstract class Item : IItem
{
    private const float BobHeight = 3.0f; // pixels
    private const float BobPeriod = 1.6f; // seconds

    private readonly BodyHurtbox _bodyHurtbox;
    private readonly Vector2 _restingPosition;
    private readonly Point _size;

    private readonly ISprite _sprite;
    private float _elapsedSeconds;

    protected Item(ItemType type, ItemArt art, Vector2 center)
    {
        Type = type;
        _sprite = art.Sprite;
        _size = art.Size;
        _restingPosition = center - new Vector2(art.Size.X / 2f, art.Size.Y / 2f);
        Position = _restingPosition;
        _bodyHurtbox = new BodyHurtbox(this, new Point(0, 0), new Point(_size.X, _size.Y));
    }

    public DamageTeam Team => DamageTeam.Neutral;
    public bool IsFacingLeft { get; }

    public IEnumerable<IHurtbox> Hurtboxes
    {
        get { yield return _bodyHurtbox; }
    }

    public bool IsDead { get; set; }

    public void TakeDamage(DamageInfo damageInfo)
    {
        // TODO Make it take damage when ran into instead of attacked
        Console.WriteLine($"{Type} took damage. Source: {damageInfo.Source}");
        IsDead = true;
    }

    public ItemType Type { get; }
    public Vector2 Position { get; private set; }
    public Rectangle Bounds => new((int)Position.X, (int)Position.Y, _size.X, _size.Y);

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

    public static List<IItem> GetAllItems(Vector2 center, int offsetX = 30)
    {
        var items = new List<IItem>();
        foreach (var type in Enum.GetValues<ItemType>())
            items.Add(ItemFactory.Create(type, center + new Vector2(offsetX * items.Count, 0)));
        return items;
    }
}