using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

/// Creates the item class that matches an ItemType.
public static class ItemFactory
{
    public static IItem Create(ItemType type, Vector2 center)
    {
        return type switch
        {
            ItemType.Boomerang => new BoomerangItem(center),
            ItemType.Bow => new BowItem(center),
            ItemType.Arrow => new ArrowItem(center),
            ItemType.Bomb => new BombItem(center),
            ItemType.Rupee => new RupeeItem(center),
            ItemType.Triforce => new TriforceItem(center),
            ItemType.Key => new KeyItem(center),
            ItemType.Heart => new HeartItem(center),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown item type")
        };
    }
}
