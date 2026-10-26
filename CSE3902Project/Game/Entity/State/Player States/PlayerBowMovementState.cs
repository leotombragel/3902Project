using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State.Player_States;

public abstract class PlayerBowMovementState : IState
{
    protected readonly Player Player;

    protected PlayerBowMovementState(Player player)
    {
        Player = player;
    }

    public abstract string AnimationName { get; }

    public virtual void Enter()
    {
        Player.IsUsingBow = true;
    }

    public abstract void Update(GameTime gameTime);

    public virtual void Exit()
    {
    }
}