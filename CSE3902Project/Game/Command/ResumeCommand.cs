using CSE3902Project.Game.State;

namespace CSE3902Project.Game.Command;

public class ResumeCommand : ICommand
{
    private readonly StateManager _stateManager;

    public ResumeCommand(StateManager stateManager)
    {
        _stateManager = stateManager;
    }

    public void Execute()
    {
        _stateManager.Pop();
    }
}