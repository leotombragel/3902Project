using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

public class ArrowItem : Item
{
    public ArrowItem(Vector2 center)
        : base(ItemType.Arrow, ItemSpriteFactory.Instance.CreateArrowArt(), center)
    {
    }
}
