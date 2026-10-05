using CSE3902Project.Game.Items;

namespace CSE3902Project.Game.Command;

public class PreviousItemCommand : ICommand
{
    private readonly ItemCycler _items;

    public PreviousItemCommand(ItemCycler items)
    {
        _items = items;
    }

    public void Execute()
    {
        _items.Previous();
    }
}
