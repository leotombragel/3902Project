using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Entity.State;
using CSE3902Project.Game.Tiles;
using System;
using System.Collections.Generic;


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
                if (wizzrobe._bodyHurtbox.Bounds.Intersects(tile.Bounds)) return true;
            }
            return false;
        }

        public void CheckForValidPosition(WizzrobeEnemy wizzrobe)
        {
            
            if (isInTile(wizzrobe)){
                wizzrobe.flipDirection();
            }
        }

    }
}
