using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Collision;

/// figures out which side of the player hit a tile
public class TileCollisionDetector
{
    //call this after moving the player sideways
    public CollisionSide GetHorizontalSide(Rectangle playerBox, Rectangle tileBox)
    {
        if (!playerBox.Intersects(tileBox))
        {
            return CollisionSide.None;
        }

        //whichever side the middle of the player is on is the side that got hit
        if (playerBox.Center.X < tileBox.Center.X)
        {
            return CollisionSide.Right;
        }

        return CollisionSide.Left;
    }

    //call this after moving the player up or down
    public CollisionSide GetVerticalSide(Rectangle playerBox, Rectangle tileBox)
    {
        if (!playerBox.Intersects(tileBox))
        {
            return CollisionSide.None;
        }

        if (playerBox.Center.Y < tileBox.Center.Y)
        {
            return CollisionSide.Bottom;
        }

        return CollisionSide.Top;
    }
}
