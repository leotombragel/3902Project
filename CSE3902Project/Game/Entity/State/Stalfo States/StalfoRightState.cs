using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class StalfoRightState : IState
{
    private readonly StalfoEnemy _stalfoEnemy;
    
    public StalfoRightState(StalfoEnemy enemy)
    {
        _stalfoEnemy = enemy;
    }

    public string AnimationName => "StalfoRight";

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