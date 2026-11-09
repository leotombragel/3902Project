using System;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Hud;

/// Row of hearts in the top left. Red hearts are the health left, gray hearts are the health lost.
public class HeartsDisplay
{
    private const int MinimumHearts = 3;
    private const int Gap = 4;

    private readonly Player _player;
    private readonly Vector2 _position;
    private readonly Sprite _fullHeart;
    private readonly Sprite _emptyHeart;

    public HeartsDisplay(Player player, Vector2 position)
    {
        _player = player;
        _position = position;
        _fullHeart = HudSpriteFactory.Instance.CreateHeart();
        _emptyHeart = HudSpriteFactory.Instance.CreateEmptyHeart();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int heartWidth = HudSpriteFactory.Instance.HeartWidth;

        //a heart pickup can push health above 3, so make room for those too
        int heartCount = Math.Max(MinimumHearts, _player.Health);

        for (int i = 0; i < heartCount; i++)
        {
            Vector2 heartPosition = _position + new Vector2(i * (heartWidth + Gap), 0);

            if (i < _player.Health)
            {
                _fullHeart.Draw(spriteBatch, heartPosition);
            }
            else
            {
                _emptyHeart.Draw(spriteBatch, heartPosition);
            }
        }
    }
}
