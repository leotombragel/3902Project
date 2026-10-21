using CSE3902Project.Game.Entity;
using Microsoft.Xna.Framework;
using System;

namespace CSE3902Project.Game.Cameras;

/// moves the camera so the player stays in the middle of the screen
public class CameraFollowPlayer
{
    private readonly Camera _camera;
    private readonly Player _player;
    private readonly Rectangle _stageBounds;

    public CameraFollowPlayer(Camera camera, Player player, Rectangle stageBounds)
    {
        _camera = camera;
        _player = player;
        _stageBounds = stageBounds;
    }

    public void Update()
    {
        var playerCenter = new Point(
            (int)Math.Round(_player.Position.X + 32),
            _player.StageHitbox.Center.Y);
        float x = playerCenter.X - _camera.ViewWidth / 2f;
        float y = playerCenter.Y - _camera.ViewHeight / 2f;

        //don't show anything past the edges of the stage
        float maxX = _stageBounds.Right - _camera.ViewWidth;
        float maxY = _stageBounds.Bottom - _camera.ViewHeight;

        if (x > maxX)
        {
            x = maxX;
        }
        if (x < _stageBounds.Left)
        {
            x = _stageBounds.Left;
        }
        if (y > maxY)
        {
            y = maxY;
        }
        if (y < _stageBounds.Top)
        {
            y = _stageBounds.Top;
        }

        _camera.Position = new Vector2(x, y);
    }
}
