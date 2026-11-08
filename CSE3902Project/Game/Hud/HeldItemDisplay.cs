using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using CSE3902Project.Game.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Hud;

/// Box that shows the item Link is holding. The box is empty when Link has nothing.
public class HeldItemDisplay
{
    private const int BoxSize = 52;

    private readonly Player _player;
    private readonly Rectangle _box;
    private IItem _icon;

    public HeldItemDisplay(Player player, Vector2 topLeft)
    {
        _player = player;
        _box = new Rectangle((int)topLeft.X, (int)topLeft.Y, BoxSize, BoxSize);
    }

    public void Update(GameTime gameTime)
    {
        //link lost the item (got hit)
        if (_player.CurrentItem == null)
        {
            _icon = null;
            return;
        }

        //link picked up something new, so make a new picture for it
        if (_icon == null || _icon.Type != _player.CurrentItem.Type)
        {
            Vector2 boxCenter = new Vector2(_box.Center.X, _box.Center.Y);
            _icon = ItemFactory.Create(_player.CurrentItem.Type, boxCenter);
        }

        _icon.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D pixel = HudSpriteFactory.Instance.Pixel;

        //border then dark middle
        spriteBatch.Draw(pixel, _box, Color.White * 0.8f);
        Rectangle inside = new Rectangle(_box.X + 2, _box.Y + 2, _box.Width - 4, _box.Height - 4);
        spriteBatch.Draw(pixel, inside, Color.Black * 0.6f);

        if (_icon != null)
        {
            _icon.Draw(spriteBatch);
        }
    }
}
