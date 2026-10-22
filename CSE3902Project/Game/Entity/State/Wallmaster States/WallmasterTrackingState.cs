using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class WallmasterTrackingState : IState
{
    private readonly WallmasterEnemy _wallmasterEnemy;
    
    public WallmasterTrackingState(WallmasterEnemy enemy)
    {
        _wallmasterEnemy = enemy;
    }

    public string AnimationName => "WallmasterTrackingState";

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