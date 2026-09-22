using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity.State;

public class PlayerJumpingState : IState
{
    private readonly Player _player;
    
    public PlayerJumpingState(Player player)
    {
        _player = player;
    }

    public string AnimationName => "PlayerJump";

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