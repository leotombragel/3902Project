using System.Collections.Generic;
using System.Linq;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Tiles;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Collision;

/// moves the player and handles them running into the tiles on the stage
public class StalfoStageCollision
{
    private readonly List<ITile> _tiles;
    private readonly StageBoundary _boundary;
    private readonly TileCollisionDetector _detector = new TileCollisionDetector();
    private readonly StalfoTileCollisionHandler _handler = new StalfoTileCollisionHandler();

    public StalfoStageCollision(List<ITile> tiles)
    {
        _tiles = tiles;
    }

    public void MoveStalfo(StalfoEnemy stalfo)
    {
        //move x first and then y so the player can slide along walls and floors
        MoveSideways(stalfo);
    }

    private void MoveSideways(StalfoEnemy stalfo)
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
