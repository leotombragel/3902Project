using Sprint0_2.Game.Entity;

namespace Sprint0_2.Game.Command;

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