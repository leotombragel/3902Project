using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State.Player_States;

public class PlayerDeadState : IState
{
    private readonly int _lastTimer = 0;
    private readonly Player _player;
    private int _timer;

    public PlayerDeadState(Player player)
    {
        _player = player;
    }

    public string AnimationName => "PlayerDead";

    public void Enter()
    {
        // Get rid of the player's hitbox
    }

    public void Update(GameTime gameTime)
    {
        _timer = gameTime.ElapsedGameTime.Milliseconds;
        // if (_timer - _lastTimer > 12) _player.Animation.Sprite.Rotation = Random.Shared.Next(0, 2 * (int)Math.PI);
        if (_timer - _lastTimer > 12) _player.Animation.Sprite.Rotation += 1;
    }

    public void Exit()
    {
    }
}