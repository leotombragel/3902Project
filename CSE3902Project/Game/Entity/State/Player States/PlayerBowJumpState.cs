using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State.Player_States;

public class PlayerBowJumpState : PlayerBowMovementState
{
    public PlayerBowJumpState(Player player)
        : base(player)
    {
    }

    public override string AnimationName => "PlayerBowJump";

    public override void Update(GameTime gameTime)
    {
        if (Player.Velocity.Y >= 0) Player.ChangeState(new PlayerBowFallState(Player));
    }
}