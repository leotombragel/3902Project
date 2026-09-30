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

        _player.Velocity = _player.Velocity with
        {
            Y = (float)(Random.Shared.NextDouble() * -5d), X = (float)(Random
                .Shared.NextDouble() * 14d - 7d)
        };
    }

    public void Update(GameTime gameTime)
    {
        _timer = gameTime.ElapsedGameTime.Milliseconds;
        if (_timer - _lastTimer > 12)
        {
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
        }
    }

    public void Exit()
    {
        _player.ShouldManagePosition = true;
    }
}