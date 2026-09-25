using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class StalfoDeadState : IState
{
    private readonly StalfoEnemy _stalfoEnemy;
    
    public StalfoDeadState(StalfoEnemy enemy)
    {
        _stalfoEnemy = enemy;
    }

    public string AnimationName => "StalfoDead";

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