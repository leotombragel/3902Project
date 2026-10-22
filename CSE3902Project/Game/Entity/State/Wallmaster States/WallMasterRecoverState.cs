using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class WallmasterRecoverState : IState
{
    private readonly WallmasterEnemy _wallmasterEnemy;
    
    public WallmasterRecoverState(WallmasterEnemy enemy)
    {
        _wallmasterEnemy = enemy;
    }

    public string AnimationName => "WallmasterRecoverState";

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