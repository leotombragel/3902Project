using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

public class KeyItem : Item
{
    public KeyItem(Vector2 center)
        : base(ItemType.Key, ItemSpriteFactory.Instance.CreateKeyArt(), center)
    {
    }
}
