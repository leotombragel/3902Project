using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class WallmasterWaitingState : IState
{
    private readonly WallmasterEnemy _wallmasterEnemy;
    
    public WallmasterWaitingState(WallmasterEnemy enemy)
    {
        _wallmasterEnemy = enemy;
    }

    public string AnimationName => "WallmasterWaitingState";

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