using CSE3902Project.Game.State;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Command;

public class PauseCommand : ICommand
{
    private readonly PauseState _pauseState;
    private readonly StateManager _stateManager;
    
    public PauseCommand(PauseState pauseState, StateManager stateManager)
    {
        _pauseState = pauseState;
        _stateManager = stateManager;
    }

    public void Execute()
    {
        _stateManager.Push(_pauseState);
    }
}