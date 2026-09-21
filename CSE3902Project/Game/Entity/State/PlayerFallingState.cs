using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class PlayerFallingState : IState
{
    private readonly Player _player;
    
    public PlayerFallingState(Player player)
    {
        _player = player;
    }

    public string AnimationName => "PlayerFall";

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