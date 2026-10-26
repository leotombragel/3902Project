using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Entity.State;
using CSE3902Project.Game.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CSE3902Project.Game.Collision
{
    public class WizzrobeCollision
    {
        private readonly List<ITile> _tiles;
        private readonly TileCollisionDetector _detector = new TileCollisionDetector();

        public WizzrobeCollision(List<ITile> tiles)
        {
            _tiles = tiles;
        }

        public Boolean isInTile(WizzrobeEnemy wizzrobe)
        {
            foreach (ITile tile in _tiles)
            {
                CollisionSide side = _detector.GetHorizontalSide(wizzrobe._bodyHurtbox.Bounds, tile.Bounds);
                if (side != CollisionSide.None)
                    return true;
            }
            return false;
        }

        public void CheckForValidPosition(WizzrobeEnemy wizzrobe)
        {
            //currently just kills the wizzrobe if it spawns in a tile
            if (isInTile(wizzrobe))
                wizzrobe.IsDead = true;
        }

    }
}
