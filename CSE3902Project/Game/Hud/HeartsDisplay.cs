using System;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Hud;

/// Row of hearts in the top left. Full hearts are the health left, dark hearts are the health lost.
public class HeartsDisplay
{
    private const int MinimumHearts = 3;
    private const int Gap = 4;

    private readonly Player _player;
    private readonly Vector2 _position;
    private readonly Sprite _heart;
    private readonly Color _lostColor = new Color(40, 40, 40) * 0.6f;

    public HeartsDisplay(Player player, Vector2 position)
    {
        _player = player;
        _position = position;
        _heart = HudSpriteFactory.Instance.CreateHeart();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int heartSize = HudSpriteFactory.Instance.IconSize;

        //a heart pickup can push health above 3, so make room for those too
        int heartCount = Math.Max(MinimumHearts, _player.Health);

        for (int i = 0; i < heartCount; i++)
        {
            if (i < _player.Health)
            {
                _heart.Color = Color.White;
            }
            else
            {
                _heart.Color = _lostColor;
            }

            Vector2 heartPosition = _position + new Vector2(i * (heartSize + Gap), 0);
            _heart.Draw(spriteBatch, heartPosition);
        }
    }
}
