using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Command;

public class SetIdlePlayerSpriteCommand(Player player) : ICommand
{
    private readonly Vector2 _scale = new(2.0f);
    private Player _player = player;

    public void Execute()
    {
        var sprite = SpriteFactory.Instance.CreateIdlePlayerSprite();
        sprite.Scale = _scale;
    }
}