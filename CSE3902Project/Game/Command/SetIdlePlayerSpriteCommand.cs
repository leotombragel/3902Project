using Microsoft.Xna.Framework;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Command;

public class SetIdlePlayerSpriteCommand(Player player) : ICommand
{
    private Player _player = player;
    
    private readonly Vector2 _scale = new(2.0f);

    public void Execute()
    {
        var sprite = SpriteFactory.Instance.CreateIdlePlayerSprite();
        sprite.Scale = _scale;
        _player.Sprite = sprite;
    }
}