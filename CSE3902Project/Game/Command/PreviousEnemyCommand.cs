using System.Collections.Generic;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Command;

public class PreviousEnemyCommand : ICommand
{
    private readonly EnemyCycler _cycler;

    public PreviousEnemyCommand(EnemyCycler cycler)
    {
        _cycler = cycler;
    }

    public void Execute() => _cycler.Previous();
}
