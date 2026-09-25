using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class StalfoLeftState : IState
{
    private readonly StalfoEnemy _stalfoEnemy;
    
    public StalfoLeftState(StalfoEnemy enemy)
    {
        _stalfoEnemy = enemy;
    }

    public string AnimationName => "StalfoLeft";

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