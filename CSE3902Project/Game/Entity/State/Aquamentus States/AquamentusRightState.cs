using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class AquamentusRightState : IState
{
    private readonly AquamentusEnemy _aquamentusEnemy;
    
    public AquamentusRightState(AquamentusEnemy enemy)
    {
        _aquamentusEnemy = enemy;
    }

    public string AnimationName => "AquamentusRight";

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