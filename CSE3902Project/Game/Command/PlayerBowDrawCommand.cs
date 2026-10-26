using System;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Entity.State.Player_States;
using CSE3902Project.Game.Items;

namespace CSE3902Project.Game.Command;

public class PlayerBowDrawCommand : ICommand
{
    private readonly Player _player;
    private readonly bool _requiresItem;

    public PlayerBowDrawCommand(Player player, bool requiresItem = true)
    {
        _player = player;
        _requiresItem = requiresItem;
    }

    public void Execute()
    {
        if (_player.IsUsingBow || (_player.CurrentItem is not BowItem && _requiresItem)) return;

        _player.ChangeState(new PlayerBowState(_player));
    }
}