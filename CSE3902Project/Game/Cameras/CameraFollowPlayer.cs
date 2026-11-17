using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Tiles;
using Microsoft.Xna.Framework;
using System;

namespace CSE3902Project.Game.Cameras;

/// moves the camera so the player stays in the middle of the screen
public class CameraFollowPlayer
{
    private readonly Camera _camera;
    private readonly Player _player;
    private readonly Rectangle _stageBounds;
    private readonly CameraBoundsArea[] _cameraBounds;

    public CameraFollowPlayer(
        Camera camera,
        Player player,
        Rectangle stageBounds,
        CameraBoundsArea[] cameraBounds)
    {
        _camera = camera;
        _player = player;
        _stageBounds = stageBounds;
        _cameraBounds = cameraBounds;
    }

    public void Update()
    {
        var playerCenter = new Point(
            (int)Math.Round(_player.Position.X + 32),
            _player.StageHitbox.Center.Y);
        var activeBounds = _stageBounds;
        foreach (var area in _cameraBounds)
        {
            if (!area.Bounds.Contains(playerCenter)) continue;
            activeBounds = area.Bounds;
            break;
        }

        float x = playerCenter.X - _camera.ViewWidth / 2f;
        float y = playerCenter.Y - _camera.ViewHeight / 2f;

        x = ClampToBounds(x, activeBounds.Left, activeBounds.Right, _camera.ViewWidth);
        y = ClampToBounds(
            y,
            activeBounds.Top,
            activeBounds.Bottom,
            _camera.ViewHeight,
            alignToMaximumWhenTooSmall: true);

        _camera.Position = new Vector2(x, y);
    }

    private static float ClampToBounds(
        float position,
        int minimum,
        int maximum,
        int viewSize,
        bool alignToMaximumWhenTooSmall = false)
    {
        var maximumPosition = maximum - viewSize;
        if (maximumPosition < minimum)
        {
            if (alignToMaximumWhenTooSmall) return maximumPosition;
            return minimum - (viewSize - (maximum - minimum)) / 2f;
        }

        return Math.Clamp(position, minimum, maximumPosition);
    }
}
