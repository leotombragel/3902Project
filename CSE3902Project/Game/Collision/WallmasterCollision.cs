using System.Collections.Generic;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Tiles;

namespace CSE3902Project.Game.Collision;

public class WallmasterStageCollision
{
    private readonly TileCollisionDetector _detector = new();
    private readonly List<ITile> _tiles;

    public WallmasterStageCollision(List<ITile> tiles)
    {
        _tiles = tiles;
    }

    public void MoveWallmaster(WallmasterEnemy wallmaster)
    {
        foreach (var tile in _tiles)
        {
            var side = _detector.GetHorizontalSide(wallmaster._bodyHurtbox.Bounds, tile.Bounds);
            if (side != CollisionSide.None) wallmaster.FlipDirection();
        }
    }
}