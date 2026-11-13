using CSE3902Project.Game.State;

namespace CSE3902Project.Game.Command;

public class VictoryCommand : ICommand
{
    private readonly VictoryState _victoryState;
    private readonly StateManager _stateManager;

    public VictoryCommand(VictoryState victoryState, StateManager stateManager)
    {
        _victoryState = victoryState;
        _stateManager = stateManager;
    }

    public void Execute()
    {
        _stateManager.Push(_victoryState);
    }
}