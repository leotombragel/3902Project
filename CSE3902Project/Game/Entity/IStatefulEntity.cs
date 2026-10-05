using CSE3902Project.Game.Entity.State;

namespace CSE3902Project.Game.Entity;

/// <summary>
///     Interface for entities that can have multiple states.
/// </summary>
public interface IStatefulEntity
{
    /// <summary>
    ///     The state the entity is currently in.
    /// </summary>
    IState CurrentState { get; }

    /// <summary>
    ///     Changes the entity's state to the given new state if different, calling the appropriate Exit and Enter methods.
    ///     Does nothing if the new state is the same as the current state.
    /// </summary>
    /// <param name="newState">The next state the entity will have.</param>
    void ChangeState(IState newState);
}