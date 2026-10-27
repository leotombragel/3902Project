using System.Collections.Generic;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Tiles;

namespace CSE3902Project.Game.Collision;

public class WizzrobeCollision
{
    private readonly TileCollisionDetector _detector = new();
    private readonly List<ITile> _tiles;

    public WizzrobeCollision(List<ITile> tiles)
    {
        _tiles = tiles;
    }

    public bool isInTile(WizzrobeEnemy wizzrobe)
    {
        foreach (var tile in _tiles)
            if (wizzrobe._bodyHurtbox.Bounds.Intersects(tile.Bounds))
                return true;
        return false;
    }

    public void CheckForValidPosition(WizzrobeEnemy wizzrobe)
    {
        if (isInTile(wizzrobe)) wizzrobe.FlipDirection();
    }
}