using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State.Player_States;

public class PlayerBowState : IState
{
    private readonly Player _player;
    private bool _animationStarted;

    public PlayerBowState(Player player)
    {
        _player = player;
    }

    public string AnimationName { get; } = "PlayerBow";

    public void Enter()
    {
        _player.IsUsingBow = true;
    }

    public void Update(GameTime gameTime)
    {
        if (!_animationStarted)
        {
            if (_player.Animation is null) return;

            _player.Animation.Reset();
            _animationStarted = true;
        }

        if (_player.Animation?.LoopCount == 0) return;

        _player.ChangeState(new PlayerBowIdleState(_player));
    }

    public void Exit()
    {
    }
}