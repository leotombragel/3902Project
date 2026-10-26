using System.Collections.Generic;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Tiles;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Collision;

/// moves the player and handles them running into the tiles on the stage
public class PlayerStageCollision
{
    private readonly List<ITile> _tiles;
    private readonly StageBoundary _boundary;
    private readonly TileCollisionDetector _detector = new TileCollisionDetector();
    private readonly PlayerTileCollisionHandler _handler = new PlayerTileCollisionHandler();

    public PlayerStageCollision(List<ITile> tiles, Rectangle stageBounds)
    {
        _tiles = tiles;
        _boundary = new StageBoundary(stageBounds);
    }

    public void MovePlayer(Player player)
    {
        //move x first and then y so the player can slide along walls and floors
        MoveSideways(player);
        MoveUpAndDown(player);
        CheckForGround(player);
    }

    private void MoveSideways(Player player)
    {
        player.Position += new Vector2(player.Velocity.X, 0);

        foreach (ITile tile in _tiles)
        {
            CollisionSide side = _detector.GetHorizontalSide(player.StageHitbox, tile.Bounds);
            if (side != CollisionSide.None)
            {
                _handler.HandleCollision(player, tile, side);
            }
        }

        _boundary.KeepInside(player);
    }

    private void MoveUpAndDown(Player player)
    {
        player.Position += new Vector2(0, player.Velocity.Y);

        //if the head is under two blocks, hit the one more over the head first
        if (player.Velocity.Y < 0)
        {
            ITile headTile = FindTileOverHead(player);
            if (headTile != null)
            {
                _handler.HandleCollision(player, headTile, CollisionSide.Top);
            }
        }

        foreach (ITile tile in _tiles)
        {
            CollisionSide side = _detector.GetVerticalSide(player.StageHitbox, tile.Bounds);
            if (side != CollisionSide.None)
            {
                _handler.HandleCollision(player, tile, side);
            }
        }
    }

    //the tile the head is hitting the most (null if none)
    private ITile FindTileOverHead(Player player)
    {
        ITile bestTile = null;
        int bestOverlap = 0;

        foreach (ITile tile in _tiles)
        {
            CollisionSide side = _detector.GetVerticalSide(player.StageHitbox, tile.Bounds);
            if (side == CollisionSide.Top)
            {
                int overlapWidth = Rectangle.Intersect(player.StageHitbox, tile.Bounds).Width;
                if (overlapWidth > bestOverlap)
                {
                    bestOverlap = overlapWidth;
                    bestTile = tile;
                }
            }
        }

        return bestTile;
    }

    private void CheckForGround(Player player)
    {
        //look 1 pixel below the feet to see if there is something to stand on
        Rectangle feetBox = player.StageHitbox;
        feetBox.Y += 1;

        bool standingOnTile = false;
        foreach (ITile tile in _tiles)
        {
            if (feetBox.Intersects(tile.Bounds))
            {
                standingOnTile = true;
            }
        }

        if (standingOnTile && player.Velocity.Y >= 0)
        {
            player.IsOnGround = true;
            player.Velocity = new Vector2(player.Velocity.X, 0);
        }
        else
        {
            player.IsOnGround = false;
        }

        player.IsJumping = !player.IsOnGround;
    }
}
