using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class WizzrobeDeadState : IState
{
    private readonly WizzrobeEnemy _wizzrobeEnemy;
    
    public WizzrobeDeadState(WizzrobeEnemy enemy)
    {
        _wizzrobeEnemy = enemy;
    }

    public string AnimationName => "WizzrobeDead";

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