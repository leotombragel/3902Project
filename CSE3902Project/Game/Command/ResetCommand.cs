using CSE3902Project.Game.State;

namespace CSE3902Project.Game.Command;

public class ResetCommand : ICommand
{
    private readonly GameplayState _game;
    public ResetCommand(GameplayState game)
    {
        _game = game;

        //private readonly Game1 _game;
        
    }

    public void Execute()
    {
        _game.ResetGame();
    }
}