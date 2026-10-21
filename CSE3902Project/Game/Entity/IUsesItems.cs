using CSE3902Project.Game.Items;

namespace CSE3902Project.Game.Entity;

public interface IUsesItems
{
    IItem CurrentItem { get; }
    void ChangeItem(IItem item);
}