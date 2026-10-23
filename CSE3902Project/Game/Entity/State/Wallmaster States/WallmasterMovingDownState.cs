using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class WallmasterMovingDownState : IState
{
    private readonly WallmasterEnemy _wallmasterEnemy;
    
    public WallmasterMovingDownState(WallmasterEnemy enemy)
    {
        _wallmasterEnemy = enemy;
    }

    public string AnimationName => "WallmasterMovingDownState";

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