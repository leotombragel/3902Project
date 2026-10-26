using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Tiles;
using Microsoft.Xna.Framework;

using System;


namespace CSE3902Project.Game.Collision
{
    internal class WallmasterTileCollisionHandler
    {
        public void HandleCollision(WallmasterEnemy wallmaster, ITile tile, CollisionSide side)
        {

            wallmaster.flipDirection();
            
        }
    }
}
