using CSE3902Project.Game.State;

namespace CSE3902Project.Game.Command;

public class GameOverCommand : ICommand
{
    private readonly GameOverState _gameOverState;
    private readonly StateManager _stateManager;

    public GameOverCommand(GameOverState gameOverState, StateManager stateManager)
    {
        _gameOverState = gameOverState;
        _stateManager = stateManager;
    }

    public void Execute()
    {
        _stateManager.Push(_gameOverState);
    }
}