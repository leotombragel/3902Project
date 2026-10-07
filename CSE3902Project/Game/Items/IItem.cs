using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Items;

/// An item in the level (boomerang, rupee, key, etc.).
public interface IItem
{
    ItemType Type { get; }

    /// The item's top left corner, including the bobbing offset.
    Vector2 Position { get; }

    /// The area the item takes up. Meant for picking up items later.
    Rectangle Bounds { get; }

    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
