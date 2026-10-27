using System.Collections.Generic;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Tiles;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Collision;

// moves stalfo and handles them running into the tiles on the stage
public class StalfoStageCollision
{
    private readonly List<ITile> _tiles;
    private readonly TileCollisionDetector _detector = new TileCollisionDetector();
    private readonly StalfoTileCollisionHandler _handler = new StalfoTileCollisionHandler();

    public StalfoStageCollision(List<ITile> tiles)
    {
        _tiles = tiles;
    }

    public void MoveSideways(StalfoEnemy stalfo)
    {
        stalfo.Position += new Vector2(stalfo.Velocity.X, 0);

        foreach (ITile tile in _tiles)
        {
             CollisionSide side = _detector.GetHorizontalSide(stalfo._bodyHurtbox.Bounds, tile.Bounds);
             if (side != CollisionSide.None)
             {
                 _handler.HandleCollision(stalfo, tile, side);
             }
        }
    }
}
