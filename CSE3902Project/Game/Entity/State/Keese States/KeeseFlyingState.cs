using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class KeeseFlyingState : IState
{
    private readonly KeeseEnemy _keeseEnemy;
    
    public KeeseFlyingState(KeeseEnemy enemy)
    {
        _keeseEnemy = enemy;
    }

    public string AnimationName => "KeeseFlying";

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