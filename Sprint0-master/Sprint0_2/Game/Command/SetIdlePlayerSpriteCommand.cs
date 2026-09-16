using Microsoft.Xna.Framework;
using Sprint0_2.Game.Entity;
using Sprint0_2.Game.Graphics;

namespace Sprint0_2.Game.Command;

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