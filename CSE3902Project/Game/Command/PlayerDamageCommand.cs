using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Entity.State.Player_States;

namespace CSE3902Project.Game.Command;

public class PlayerDamageCommand : ICommand
{
    private readonly Player _player;
    
    public PlayerDamageCommand(Player player)
    {
        _player = player;
    }
    
    public void Execute()
    {
        _player.ChangeState(new PlayerDeadState(_player));
    }
}