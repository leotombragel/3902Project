using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class PlayerStabState : IState
{
    private readonly Player _player;

    public PlayerStabState(Player player)
    {
        _player = player;
    }

    public string AnimationName => "PlayerStab";

    public void Enter()
    {
        // Do something with hitbox
    }

    public void Update(GameTime gameTime)
    {
        // Use the animation to determine when the attack is finished
        if (_player.Animation?.LoopCount > 0) _player.ChangeState(new PlayerIdleState(_player));
    }

    public void Exit()
    {
        // Revert hitbox to normal
    }
}