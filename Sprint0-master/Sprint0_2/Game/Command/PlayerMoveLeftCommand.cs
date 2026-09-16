using Sprint0_2.Game.Entity;
using Sprint0_2.Game.Graphics;

namespace Sprint0_2.Game.Command;

public class PlayerMoveLeftCommand : ICommand
{
    private readonly Player _player;

    public PlayerMoveLeftCommand(Player player)
    {
        _player = player;
    }

    public void Execute()
    {
        _player.MoveHorizontal(false);
        var walkingEvent = new SetWalkingPlayerSpriteCommand(_player);
        walkingEvent.Execute();
    }
}