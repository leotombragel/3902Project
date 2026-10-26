using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Tiles;
using System;
using System.Collections.Generic;

namespace CSE3902Project.Game.Collision
{
    public class WallmasterStageCollision
    {
        private readonly List<ITile> _tiles;
        private readonly TileCollisionDetector _detector = new TileCollisionDetector();

        public WallmasterStageCollision(List<ITile> tiles)
        {
            _tiles = tiles;
        }

        public void MoveWallmaster(WallmasterEnemy wallmaster)
        {
            foreach (ITile tile in _tiles)
            {
             CollisionSide side = _detector.GetHorizontalSide(wallmaster._bodyHurtbox.Bounds, tile.Bounds);
             if (side != CollisionSide.None)
                {
                    wallmaster.flipDirection();
                }
            }
        }
    }
}
