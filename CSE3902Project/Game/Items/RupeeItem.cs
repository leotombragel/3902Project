using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

public class RupeeItem : Item
{
    public RupeeItem(Vector2 center)
        : base(ItemType.Rupee, ItemSpriteFactory.Instance.CreateRupeeArt(), center)
    {
    }
}
