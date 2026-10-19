using CSE3902Project.Game.Entity;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Collision;

/// keeps the player from walking off the left or right edge of the stage
public class StageBoundary
{
    private readonly Rectangle _bounds;

    public StageBoundary(Rectangle bounds)
    {
        _bounds = bounds;
    }

    public void KeepInside(Player player)
    {
        Rectangle box = player.StageHitbox;

        if (box.Left < _bounds.Left)
        {
            player.Position += new Vector2(_bounds.Left - box.Left, 0);
            player.Velocity = new Vector2(0, player.Velocity.Y);
        }

        if (box.Right > _bounds.Right)
        {
            player.Position -= new Vector2(box.Right - _bounds.Right, 0);
            player.Velocity = new Vector2(0, player.Velocity.Y);
        }
    }
}
