using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class WallmasterMovingUpState : IState
{
    private readonly WallmasterEnemy _wallmasterEnemy;
    
    public WallmasterMovingUpState(WallmasterEnemy enemy)
    {
        _wallmasterEnemy = enemy;
    }

    public string AnimationName => "WallmasterMovingUpState";

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