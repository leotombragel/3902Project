using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class AquamentusLivingState : IState
{
    private readonly AquamentusEnemy _aquamentusEnemy;

    public AquamentusLivingState(AquamentusEnemy enemy)
    {
        _aquamentusEnemy = enemy;
    }

    public string AnimationName => "AquaementusLiving";

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