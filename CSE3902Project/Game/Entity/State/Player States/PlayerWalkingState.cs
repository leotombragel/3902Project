using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class PlayerWalkingState : StateBase
{
    private readonly Player _player;
    
    public PlayerWalkingState(Player player)
    {
        _player = player;
    }

    public override string AnimationName => "PlayerWalk";

    public override void Update(GameTime gameTime)
    {
        if (_player.Velocity.Y > 0)
        {
            _player.ChangeState(new PlayerFallingState(_player));
        }
        
        if (Math.Abs(_player.Velocity.X) > 0)
        {
            return;
        }
        
        _player.ChangeState(new PlayerIdleState(_player));
    }
}