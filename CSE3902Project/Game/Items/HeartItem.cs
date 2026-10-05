using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

public class HeartItem : Item
{
    public HeartItem(Vector2 center)
        : base(ItemType.Heart, ItemSpriteFactory.Instance.CreateHeartArt(), center)
    {
    }
}
