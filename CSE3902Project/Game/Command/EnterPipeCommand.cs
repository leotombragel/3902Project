using System;

namespace CSE3902Project.Game.Command;

public sealed class EnterPipeCommand : ICommand
{
    private readonly Action _enterPipe;

    public EnterPipeCommand(Action enterPipe)
    {
        ArgumentNullException.ThrowIfNull(enterPipe);
        _enterPipe = enterPipe;
    }

    public void Execute()
    {
        _enterPipe();
    }
}
