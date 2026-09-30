using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class WizzrobeInvisibleState : IState
{
    private readonly WizzrobeEnemy _wizzrobeEnemy;
    
    public WizzrobeInvisibleState(WizzrobeEnemy enemy)
    {
        _wizzrobeEnemy = enemy;
    }

    public string AnimationName => "WizzrobeInvisible";

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