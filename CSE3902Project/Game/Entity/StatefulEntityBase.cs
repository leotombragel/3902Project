using CSE3902Project.Game.Entity.State;

namespace CSE3902Project.Game.Entity;

public class StatefulEntityBase : IStatefulEntity
{
    public IState CurrentState { get; protected set; }
    public void ChangeState(IState newState)
    {
        // Ignore state change if re-entering the current state
        if (newState.GetType() == CurrentState.GetType()) return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
}