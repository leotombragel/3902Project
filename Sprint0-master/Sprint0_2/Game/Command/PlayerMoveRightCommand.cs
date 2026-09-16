using Sprint0_2.Game.Entity;

namespace Sprint0_2.Game.Command;

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
        var walkingEvent = new SetWalkingPlayerSpriteCommand(_player);
        walkingEvent.Execute();
    }
}