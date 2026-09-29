using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Entity.State;

namespace CSE3902Project.Game.Command;

public class PlayerAttackCommand : ICommand
{
    private readonly Player _player;

    public PlayerAttackCommand(Player player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.ChangeState(new PlayerStabState(_player));
    }
}