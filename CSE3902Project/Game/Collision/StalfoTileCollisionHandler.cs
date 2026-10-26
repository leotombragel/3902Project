using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE3902Project.Game.Collision
{
    public class StalfoTileCollisionHandler
    {
        public void HandleCollision(StalfoEnemy stalfo, ITile tile, CollisionSide side)
        {
            Rectangle overlap = Rectangle.Intersect(stalfo._bodyHurtbox.Bounds, tile.Bounds);

            if (side == CollisionSide.Right)
            {
                stalfo.Position -= new Vector2(overlap.Width, 0);
                stalfo.flipDirection();
            }
            else if (side == CollisionSide.Left)
            {
                stalfo.Position += new Vector2(overlap.Width, 0);
                stalfo.flipDirection();
            }

            //if (stalfo._bodyHurtbox.Bounds.Intersects(tile.Bounds))
            //{
            //    stalfo.flipDirection();
            //    if (stalfo.isFacingLeft)
            //    {
            //        stalfo.Position
            //    }
            //}
             
        }
    }
}
