using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

public class ArrowItem : Item
{
    public ArrowItem(Vector2 center)
        : base(ItemType.Bow, ItemSpriteFactory.Instance.CreateArrowArt(), center)
        //unlimited ammo so arrow item should just give bow
        //in the future we should use one or the other, bow or arrow item
    {
    }
}
