using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Tiles;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Collision;

/// pushes the player back out of a tile after they run into it
public class PlayerTileCollisionHandler
{
    public void HandleCollision(Player player, ITile tile, CollisionSide side)
    {
        //the overlap is how far we have to push the player back out
        Rectangle overlap = Rectangle.Intersect(player.StageHitbox, tile.Bounds);

        if (side == CollisionSide.Right)
        {
            player.Position -= new Vector2(overlap.Width, 0);
            player.Velocity = new Vector2(0, player.Velocity.Y);
        }
        else if (side == CollisionSide.Left)
        {
            player.Position += new Vector2(overlap.Width, 0);
            player.Velocity = new Vector2(0, player.Velocity.Y);
        }
        else if (side == CollisionSide.Bottom)
        {
            //landed on the tile
            player.Position -= new Vector2(0, overlap.Height);
            player.Velocity = new Vector2(player.Velocity.X, 0);
            player.IsOnGround = true;
            player.SwitchToInactiveState();
        }
        else if (side == CollisionSide.Top)
        {
            //bumped head
            player.Position += new Vector2(0, overlap.Height);
            player.Velocity = new Vector2(player.Velocity.X, 0);
        }
    }
}
