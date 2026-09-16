using System.Net.Mime;
using Microsoft.Xna.Framework;
using Sprint0_2.Game.Entity;
using Sprint0_2.Game.Graphics;

namespace Sprint0_2.Game.Command;

public class SetWalkingPlayerSpriteCommand(Player player) : ICommand
{
    private Player _player = player;
    
    // Configuration to make the sprite look correct
    private readonly Vector2 _scale = new(2.0f);
    private readonly int _width = 39;
    private readonly int _height = 44;
    private readonly int _count = 16;
    private readonly float _duration = 0.08f;
    
    public void Execute()
    {
        if (_player.IsWalking) return;
        var sprite = SpriteFactory.Instance.CreateWalkingPlayerSprite();
        _player.Sprite = sprite;
        sprite.Scale = _scale;

        _player.Animation = new SpriteAnimation(sprite, _width, _height, _count, _duration);

        _player.IsWalking = true;
    }
}   