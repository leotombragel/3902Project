using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Items;

public class TorchItem : Item
{
    public TorchItem(Vector2 center)
        : base(ItemType.Torch, ItemSpriteFactory.Instance.CreateTorchArt(), center)
    {
    }
}