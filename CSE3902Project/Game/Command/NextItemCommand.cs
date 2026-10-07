using CSE3902Project.Game.Items;

namespace CSE3902Project.Game.Command;

public class NextItemCommand : ICommand
{
    private readonly ItemCycler _items;

    public NextItemCommand(ItemCycler items)
    {
        _items = items;
    }

    public void Execute()
    {
        _items.Next();
    }
}
