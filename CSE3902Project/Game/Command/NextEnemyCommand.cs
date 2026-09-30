using System.Collections.Generic;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Command;

public class NextEnemyCommand : ICommand
{
    private readonly EnemyCycler _cycler;

    public NextEnemyCommand(EnemyCycler cycler)
    {
        _cycler = cycler;
    }

    public void Execute() => _cycler.Next();
}
