using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class AquamentusDeadState : IState
{
    private readonly AquamentusEnemy _aquamentusEnemy;

    public AquamentusDeadState(AquamentusEnemy enemy)
    {
        _aquamentusEnemy = enemy;
    }

    public string AnimationName => "AquaementusDead";

    public void Enter()
    {
    }

    public void Update(GameTime gameTime)
    {
        
    }

    public void Exit()
    {
    }
}