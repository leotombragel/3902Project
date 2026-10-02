using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State.Player_States;

public class PlayerDeadState : IState
{
    private readonly int _lastTimer = 0;
    private readonly Player _player;
    private int _timer;

    public PlayerDeadState(Player player)
    {
        _player = player;
    }

    public string AnimationName => "PlayerDead";

    public void Enter()
    {
        // Get rid of the player's hitbox

        // Sets player velocity to upwards and to some random horizontal direction.
        // This class's Update method adds extra gravity so he falls off the screen quickly, and we let Player.Decelerate
        // handle the horizontal deceleration.
        _player.Velocity = _player.Velocity with
        {
            Y = (float)(Random.Shared.NextDouble() * -5d), X = (float)(Random
                .Shared.NextDouble() * 14d - 7d)
        };
    }

    public void Update(GameTime gameTime)
    {
        _timer = gameTime.ElapsedGameTime.Milliseconds;

        // This should be changed to delta time later
        if (_timer - _lastTimer <= 12) return;

        _timer = 0;

        var sprite = _player.Animation.Sprite;

        // Height equals width for player dead sprite
        var width = sprite.SourceRectangle?.Width ?? 16f;

        sprite.Rotation += 0.1f;
        sprite.Origin = new Vector2(width * 0.5f, width * 0.5f);

        // Fake gravity
        _player.Velocity = _player.Velocity with
        {
            Y = _player.Velocity.Y + 0.1f
        };

        // Reset player once he falls off the screen. Later we can also do things like subtracting lives.
        if (_player.Position.Y > 1000)
        {
            _player.ShouldManagePosition = true;

            _player.ChangeState(new PlayerIdleState(_player));
        }
    }

    public void Exit()
    {
        _player.Position = new Vector2(200, 100);
        _player.Velocity = new Vector2(0, 0);
    }
}