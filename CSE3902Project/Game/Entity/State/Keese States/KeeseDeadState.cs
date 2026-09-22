using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class KeeseDeadState : IState
{
    private readonly KeeseEnemy _keeseEnemy;
    
    public KeeseDeadState(KeeseEnemy enemy)
    {
        _keeseEnemy = enemy;
    }

    public string AnimationName => "KeeseDead";

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