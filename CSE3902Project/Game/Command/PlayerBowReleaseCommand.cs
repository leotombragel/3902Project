using System;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Entity.Projectiles;
using CSE3902Project.Game.Graphics;
using CSE3902Project.Game.Items;

namespace CSE3902Project.Game.Command;

public sealed class PlayerBowReleaseCommand : ICommand
{
    private readonly Player _player;
    private readonly Action<IProjectile> _spawnProjectile;

    public PlayerBowReleaseCommand(
        Player player,
        Action<IProjectile> spawnProjectile)
    {
        _player = player;
        _spawnProjectile = spawnProjectile;
    }

    public void Execute()
    {
        Console.WriteLine("Here are the conditions: " + _player.IsUsingBow + ", " + (_player.CurrentItem is ArrowItem) + ", " + (_player.CurrentItem is BowItem));
        if (!_player.IsUsingBow || _player.CurrentItem is not BowItem) return;

        if (_player.CurrentState.AnimationName != "PlayerBow") // PlayerBow is for when they are drawing the bow
            _spawnProjectile(new ArrowProjectile(
                (int)_player.Position.X,
                (int)_player.Position.Y,
                _player.IsFacingLeft,
                _player
            ));


        _player.ReleaseBow();
    }
}