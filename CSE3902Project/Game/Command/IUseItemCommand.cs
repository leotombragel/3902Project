using System;

namespace CSE3902Project.Game.Command;

/// <summary>
///     Interface for commands that make the player use an item. These commands store the type of item,
///     which makes it easier to find the correct item to use.
/// </summary>
public interface IUseItemCommand : ICommand
{
    Type ItemType { get; }
}