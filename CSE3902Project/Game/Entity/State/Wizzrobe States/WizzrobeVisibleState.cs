using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class WizzrobeVisibleState : IState
{
    private readonly WizzrobeEnemy _wizzrobeEnemy;
    
    public WizzrobeVisibleState(WizzrobeEnemy enemy)
    {
        _wizzrobeEnemy = enemy;
    }

    public string AnimationName => "WizzrobeVisible";

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