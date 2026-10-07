using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

public class BowItem : Item
{
    public BowItem(Vector2 center)
        : base(ItemType.Bow, ItemSpriteFactory.Instance.CreateBowArt(), center)
    {
    }
}
