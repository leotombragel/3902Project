using CSE3902Project.Game.Tiles;

namespace CSE3902Project.Game.Command;

public class PreviousTileCommand : ICommand
{
    private readonly TileCycler _tiles;

    public PreviousTileCommand(TileCycler tiles)
    {
        _tiles = tiles;
    }

    public void Execute()
    {
        _tiles.Previous();
    }
}
