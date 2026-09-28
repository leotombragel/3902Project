using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class KeeseStoppedState : IState
{
    private readonly KeeseEnemy _keeseEnemy;
    
    public KeeseStoppedState(KeeseEnemy enemy)
    {
        _keeseEnemy = enemy;
    }

    public string AnimationName => "KeeseStopped";

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