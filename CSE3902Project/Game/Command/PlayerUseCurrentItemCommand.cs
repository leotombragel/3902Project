using System;
using System.Collections.Generic;
using System.Linq;
using CSE3902Project.Game.Entity;

namespace CSE3902Project.Game.Command;

public class PlayerUseCurrentItemCommand : ICommand
{
    private readonly List<IUseItemCommand> _itemCommands;
    private readonly Player _player;

    public PlayerUseCurrentItemCommand(Player player, List<IUseItemCommand> itemCommands)
    {
        _player = player;
        _itemCommands = itemCommands;
    }

    public void Execute()
    {
        if (_player.CurrentItem == null) return;

        // Use the command matching the player's current item
        foreach (var command in _itemCommands.Where(command => command.ItemType == _player.CurrentItem.GetType()))
        {
            command.Execute();
            break;
        }
    }
}