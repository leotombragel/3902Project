using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

public class TriforceItem : Item
{
    public TriforceItem(Vector2 center)
        : base(ItemType.Triforce, ItemSpriteFactory.Instance.CreateTriforceArt(), center)
    {
    }
}
