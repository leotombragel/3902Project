using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class PlayerIdleState : StateBase
{
    private readonly Player _player;
    
    public PlayerIdleState(Player player)
    {
        _player = player;
    }
    
    public override string AnimationName => "PlayerIdle";
    
    public override void Update(GameTime gameTime)
    {
        if (_player.Velocity.X != 0)
        {
            _player.ChangeState(new PlayerWalkingState(_player));
        }
    }
}