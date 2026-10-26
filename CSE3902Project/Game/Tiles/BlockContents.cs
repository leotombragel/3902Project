using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Tiles;

/// the item hiding inside a block
public class BlockContents
{
    private readonly StageCellType _itemType;
    private readonly List<ItemSpawn> _poppedItems;
    private bool _hasItem;

    //a block with nothing inside
    public BlockContents()
    {
        _hasItem = false;
    }

    public BlockContents(StageCellType itemType, List<ItemSpawn> poppedItems)
    {
        _itemType = itemType;
        _poppedItems = poppedItems;
        _hasItem = true;
    }

    public bool HasItem
    {
        get { return _hasItem; }
    }

    //puts the item above the block (only comes out once)
    public void PopOut(Vector2 blockPosition)
    {
        if (!_hasItem)
        {
            return;
        }

        Vector2 itemPosition = blockPosition - new Vector2(0, Tile.Size);
        _poppedItems.Add(new ItemSpawn(_itemType, itemPosition));
        _hasItem = false;
    }
}
