using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

public class BombItem : Item
{
    public BombItem(Vector2 center)
        : base(ItemType.Bomb, ItemSpriteFactory.Instance.CreateBombArt(), center)
    {
    }
}
