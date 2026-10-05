using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

public class BoomerangItem : Item
{
    public BoomerangItem(Vector2 center)
        : base(ItemType.Boomerang, ItemSpriteFactory.Instance.CreateBoomerangArt(), center)
    {
    }
}
