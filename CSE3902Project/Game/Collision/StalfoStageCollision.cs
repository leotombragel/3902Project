using System;
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
             CollisionSide HorizontalSide = _detector.GetHorizontalSide(stalfo._bodyHurtbox.Bounds, tile.Bounds);
             if (HorizontalSide != CollisionSide.None)
             {
                 _handler.HandleCollision(stalfo, tile, HorizontalSide);
             }else{
                //make a little box that check if stalfo is about to try and cross a gap, if so flip direction
                var GroundCheckLeft = new Rectangle(stalfo._bodyHurtbox.Bounds.X - 3, stalfo._bodyHurtbox.Bounds.Y + stalfo._bodyHurtbox.Bounds.Height, 1, 1);
                CollisionSide GroundCheckLeftSide = _detector.GetHorizontalSide(GroundCheckLeft, tile.Bounds);
                if (GroundCheckLeftSide != CollisionSide.None)
                {
                    stalfo.FlipDirection();
                }
                //make a little box that check if stalfo is about to try and cross a gap, if so flip direction
                var GroundCheckRight = new Rectangle(stalfo._bodyHurtbox.Bounds.X + stalfo._bodyHurtbox.Bounds.Width + 3, stalfo._bodyHurtbox.Bounds.Y + stalfo._bodyHurtbox.Bounds.Height, 1, 1);
                CollisionSide GroundCheckRightSide = _detector.GetHorizontalSide(GroundCheckRight, tile.Bounds);
                if (GroundCheckRightSide != CollisionSide.None)
                {
                    stalfo.FlipDirection();
                }
             }
        
        }
    }
}
