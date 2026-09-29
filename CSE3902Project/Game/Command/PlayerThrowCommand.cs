using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Entity.State;

namespace CSE3902Project.Game.Command;

public class PlayerThrowCommand : ICommand
{
    private readonly Player _player;

    public PlayerThrowCommand(Player player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.ChangeState(new PlayerThrowState(_player));
    }
}