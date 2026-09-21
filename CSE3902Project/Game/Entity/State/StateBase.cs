using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

/// <summary>
///     StateBase implements IState with empty virtual Enter and Exit methods. This is meant for simpler states that don't
///     need to put logic there.
/// </summary>
public abstract class StateBase : IState
{
    public abstract string AnimationName { get; }

    public virtual void Enter()
    {
    }

    public abstract void Update(GameTime gameTime);

    public virtual void Exit()
    {
    }
}