using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State.Player_States;

public class PlayerBowFallState : PlayerBowMovementState
{
    public PlayerBowFallState(Player player)
        : base(player)
    {
    }

    public override string AnimationName => "PlayerBowFall";

    public override void Update(GameTime gameTime)
    {
        if (!Player.IsOnGround) return;

        if (Math.Abs(Player.Velocity.X) > 0)
            Player.ChangeState(new PlayerBowWalkState(Player));
        else
            Player.ChangeState(new PlayerBowIdleState(Player));
    }
}