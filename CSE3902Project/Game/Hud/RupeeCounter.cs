using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Hud;

/// Rupee picture and how many the player has, in the top right.
public class RupeeCounter
{
    private readonly Player _player;
    private readonly Vector2 _position;
    private readonly Sprite _rupee;

    public RupeeCounter(Player player, Vector2 position)
    {
        _player = player;
        _position = position;
        _rupee = HudSpriteFactory.Instance.CreateRupee();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _rupee.Draw(spriteBatch, _position);

        //text goes to the right of the picture (dark copy underneath so it is easy to read)
        string text = "x " + _player.Rupees;
        Vector2 textPosition = _position + new Vector2(HudSpriteFactory.Instance.IconSize + 6, 4);
        TextCreator.CreateText(text, textPosition + new Vector2(1, 1), spriteBatch, Color.Black);
        TextCreator.CreateText(text, textPosition, spriteBatch, Color.White);
    }
}
