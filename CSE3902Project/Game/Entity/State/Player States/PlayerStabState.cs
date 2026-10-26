using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State.Player_States;

public class PlayerStabState : IState
{
    private readonly Player _player;
    private bool _animationStarted;

    public PlayerStabState(Player player)
    {
        _player = player;
    }

    public string AnimationName => "PlayerStab";

    public void Enter()
    {
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

        _player.SwitchToInactiveState();
    }

    public void Exit()
    {
    }
}