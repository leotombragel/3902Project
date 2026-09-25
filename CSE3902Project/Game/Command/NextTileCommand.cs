using CSE3902Project.Game.Tiles;

namespace CSE3902Project.Game.Command;

public class NextTileCommand : ICommand
{
    private readonly TileCycler _tiles;

    public NextTileCommand(TileCycler tiles)
    {
        _tiles = tiles;
    }

    public void Execute()
    {
        _tiles.Next();
    }
}
