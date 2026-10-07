namespace CSE3902Project.Game.Command;

public class ResetCommand : ICommand
{
    private readonly Game1 _game;
    public ResetCommand(Game1 game)
    {
        _game = game;

        //private readonly Game1 _game;
        
    }

    public void Execute()
    {
        _game.ResetGame();
    }
}