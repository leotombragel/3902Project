using CSE3902Project.Game.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Hud;

/// Everything drawn on top of the screen: hearts (top left), rupees and held item (top right).
public class Hud
{
    private const int Margin = 12;

    private readonly HeartsDisplay _hearts;
    private readonly RupeeCounter _rupees;
    private readonly HeldItemDisplay _heldItem;

    public Hud(Player player, int screenWidth)
    {
        _hearts = new HeartsDisplay(player, new Vector2(Margin, Margin));
        _rupees = new RupeeCounter(player, new Vector2(screenWidth - 100, Margin));
        _heldItem = new HeldItemDisplay(player, new Vector2(screenWidth - 64, 52));
    }

    public void Update(GameTime gameTime)
    {
        _heldItem.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _hearts.Draw(spriteBatch);
        _rupees.Draw(spriteBatch);
        _heldItem.Draw(spriteBatch);
    }
}
