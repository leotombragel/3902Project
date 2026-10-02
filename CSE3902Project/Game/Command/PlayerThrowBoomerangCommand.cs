using System;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Entity.State;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Command;

public class PlayerThrowBoomerangCommand : ICommand
{
    private readonly Player _player;
    private readonly Action<IProjectile> _spawnProjectile;

    public PlayerThrowBoomerangCommand(Player player, Action<IProjectile> spawnProjectile)
    {
        _player = player;
        _spawnProjectile = spawnProjectile;
    }

    public void Execute()
    {
        if (_player.CurrentState is PlayerThrowState) return;

        _player.ChangeState(new PlayerThrowState(_player));
        _spawnProjectile(new BoomerangProjectile(
            _player,
            (int)_player.Position.X,
            (int)_player.Position.Y,
            _player.IsFacingLeft));
    }
}