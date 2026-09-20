using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class PlayerWalkingState : IState
{
    private readonly Player _player;
    
    public PlayerWalkingState(Player player)
    {
        _player = player;
    }

    public string AnimationName => "PlayerWalk";
    
    public void Enter()
    {
        
    }

    public void Update(GameTime gameTime)
    {
        if (_player.Velocity.X > 0)
        {
            return;
        }
        
        _player.CurrentState = new PlayerIdleState(_player);
    }

    public void Exit()
    {
        
    }
}