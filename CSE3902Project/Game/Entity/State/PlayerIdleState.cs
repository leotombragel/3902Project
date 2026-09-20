namespace CSE3902Project.Game.Entity.State;

public class PlayerIdleState : IState
{
    private readonly Player _player;
    
    public PlayerIdleState(Player player)
    {
        _player = player;
    }
    
    public string AnimationName => "PlayerIdle";
    
    public void Enter()
    {
        
    }
    
    public void Update(float deltaTime)
    {
        if (_player.Velocity.X != 0)
        {
            _player.CurrentState = new PlayerWalkingState(_player);
        }
    }
    
    public void Exit()
    {
        
    }
}