using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State.Player_States;

public class PlayerBowIdleState : PlayerBowMovementState
{
    public PlayerBowIdleState(Player player)
        : base(player)
    {
    }

    public override string AnimationName => "PlayerBowIdle";

    public override void Update(GameTime gameTime)
    {
        switch (Player.Velocity.Y)
        {
            case < 0:
                Player.ChangeState(new PlayerBowJumpState(Player));
                return;
            case > 0:
                Player.ChangeState(new PlayerBowFallState(Player));
                return;
        }

        if (Math.Abs(Player.Velocity.X) > 0) Player.ChangeState(new PlayerBowWalkState(Player));
    }
}