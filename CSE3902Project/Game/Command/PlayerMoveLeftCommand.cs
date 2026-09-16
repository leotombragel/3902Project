using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Command;

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