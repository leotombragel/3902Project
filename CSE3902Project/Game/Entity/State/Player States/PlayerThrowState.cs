using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class PlayerThrowState : IState
{
    private readonly Player _player;
    private const float LastFrameHoldDuration = 0.3f;
    private float _lastFrameHoldTime;
    private bool _animationStarted;

    public PlayerThrowState(Player player)
    {
        _player = player;
    }

    public string AnimationName => "PlayerThrow";

    public void Enter()
    {
        // Do something with hitbox
    }

    public void Update(GameTime gameTime)
    {
        if (!_animationStarted)
        {
            if (_player.Animation is null) return;

            _player.Animation.Reset();
            _animationStarted = true;
        }

        if (_player.Animation?.LoopCount == 0) return;

        _lastFrameHoldTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_lastFrameHoldTime >= LastFrameHoldDuration)
        {
            _player.ChangeState(new PlayerIdleState(_player));
        }
    }

    public void Exit()
    {
        // Revert hitbox to normal
    }
}