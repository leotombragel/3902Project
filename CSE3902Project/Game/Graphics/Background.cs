using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// The scenery behind everything else. Draw this first.
public class Background
{
    private readonly Sprite _sprite;

    public Background(int width, int height)
    {
        _sprite = BackgroundSpriteFactory.Instance.CreateBackgroundSprite(width, height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch, Vector2.Zero);
    }
}
