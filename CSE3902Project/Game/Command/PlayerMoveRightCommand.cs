using CSE3902Project.Game.Entity;

namespace CSE3902Project.Game.Command;

public class PlayerMoveRightCommand : ICommand
{
    private readonly Player _player;

    public PlayerMoveRightCommand(Player player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.MoveHorizontal(true);
    }
}