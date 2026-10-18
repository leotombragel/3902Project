using System;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Entity.State;

namespace CSE3902Project.Game.Command;

public class PlayerAttackCommand : ICommand
{
    private readonly Player _player;
    private readonly Action<IAttack> _spawnAttack;

    public PlayerAttackCommand(Player player, Action<IAttack> spawnAttack)
    {
        _player = player;
        _spawnAttack = spawnAttack;
    }

    public void Execute()
    {
        if (_player.CurrentState is PlayerStabState)
        {
            return;
        }
        
        _player.ChangeState(new PlayerStabState(_player));
        _spawnAttack(new SwordAttack(_player));
    }
}