using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State.Player_States;

public class PlayerTriforceState : IState
{
    private Player _player;
    
    public PlayerTriforceState(Player player)
    {
        _player = player;
    }
    
    public string AnimationName => "PlayerTriforce";

    public void Enter()
    {
        _player.AcceptsStateChanges = false;
        _player.ShouldManagePosition = false;
        _player.Velocity = Vector2.Zero;
        foreach (var hurtbox in _player.Hurtboxes)
        {
            hurtbox.IsEnabled = false;
        }
    }

    public void Update(GameTime gameTime)
    {
        _player.Velocity = Vector2.Zero;
        if (_player.Animation.LoopCount >= 1)
        {
            _player.FinishedLevel = true;
        }
    }

    public void Exit()
    {
    }
}