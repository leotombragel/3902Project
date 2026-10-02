using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class AquamentusLeftState : IState
{
    private readonly AquamentusEnemy _aquamentusEnemy;
    
    public AquamentusLeftState(AquamentusEnemy enemy)
    {
        _aquamentusEnemy = enemy;
    }

    public string AnimationName => "AquamentusLeft";

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