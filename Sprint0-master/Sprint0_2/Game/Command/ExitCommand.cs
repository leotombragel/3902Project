namespace Sprint0_2.Game.Command;

public class ExitCommand : ICommand
{
    private readonly Microsoft.Xna.Framework.Game _game;
    
    public ExitCommand(Microsoft.Xna.Framework.Game game)
    {
        _game = game;
    }

    public void Execute()
    {
        _game.Exit();
    }
}