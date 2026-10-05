using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// Creates the sprite for the background image.
public class BackgroundSpriteFactory
{
    public static BackgroundSpriteFactory Instance { get; } = new BackgroundSpriteFactory();

    private Texture2D _background;

    public void LoadAllAssets(ContentManager content)
    {
        _background = content.Load<Texture2D>("images/background");
    }

    /// Stretches the image to fill the given size.
    public Sprite CreateBackgroundSprite(int width, int height)
    {
        return new Sprite(_background)
        {
            Scale = new Vector2(width / (float)_background.Width, height / (float)_background.Height)
        };
    }
}
