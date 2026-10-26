using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE3902Project.Game.Collision
{
    public class WallmasterStageCollision
    {
        private readonly List<ITile> _tiles;
        private readonly StageBoundary _boundary;
        private readonly TileCollisionDetector _detector = new TileCollisionDetector();
        private readonly StalfoTileCollisionHandler _handler = new StalfoTileCollisionHandler();

        public WallmasterStageCollision(List<ITile> tiles)
        {
            _tiles = tiles;
        }

        public void MoveWallmaster(WallmasterEnemy stalfo)
        {
            //to be implemented
        }
    }
}
