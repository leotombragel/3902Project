using CSE3902Project.Game.Entity;

namespace CSE3902Project.Game.Command;

public class PlayerJumpCommand : ICommand
{
    private readonly Player _player;
    
    public PlayerJumpCommand(Player player)
    {
        _player = player;
    }
    
    public void Execute()
    {
        _player.MoveVertical();
    }
}