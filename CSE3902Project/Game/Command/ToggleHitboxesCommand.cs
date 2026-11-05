using System;

namespace CSE3902Project.Game.Command;

public sealed class ToggleHitboxesCommand : ICommand
{
    private readonly Action _toggleHitboxes;

    public ToggleHitboxesCommand(Action toggleHitboxes)
    {
        ArgumentNullException.ThrowIfNull(toggleHitboxes);
        _toggleHitboxes = toggleHitboxes;
    }

    public void Execute()
    {
        _toggleHitboxes();
    }
}
