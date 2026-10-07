using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class PlayerStabState : IState
{
    private readonly Player _player;
    private readonly int _animLoopCount;

    public PlayerStabState(Player player)
    {
        _player = player;
        
        // Animations can be null when the game launches so we need to use 0 as a fallback.
        _animLoopCount = _player.Animation?.LoopCount ?? 0;
    }

    public string AnimationName => "PlayerStab";

    public void Enter()
    {
        // Do something with hitbox
    }

    public void Update(GameTime gameTime)
    {
        // Use the animation to determine when the attack is finished
        if (_player.Animation?.LoopCount != _animLoopCount)
        {
            _player.SwitchToInactiveState();
        }
    }

    public void Exit()
    {
        // Revert hitbox to normal
    }
}