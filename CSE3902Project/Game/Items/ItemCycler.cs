using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Items;

/// Shows one item at a time. The u and i keys cycle through every item type.
public class ItemCycler
{
    private static readonly ItemType[] AllTypes = Enum.GetValues<ItemType>();

    private readonly Vector2 _center;
    private int _index;
    private IItem _currentItem;

    public ItemCycler(Vector2 center)
    {
        _center = center;
        Reset();
    }

    public void Next() => Select(_index + 1);

    public void Previous() => Select(_index - 1);

    public void Reset() => Select(0);

    public void Update(GameTime gameTime)
    {
        _currentItem.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _currentItem.Draw(spriteBatch);
    }

    private void Select(int index)
    {
        _index = (index % AllTypes.Length + AllTypes.Length) % AllTypes.Length;
        _currentItem = ItemFactory.Create(AllTypes[_index], _center);
    }
}
