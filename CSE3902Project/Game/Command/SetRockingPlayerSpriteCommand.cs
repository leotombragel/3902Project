using Microsoft.Xna.Framework;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Command;

public class SetRockingPlayerSpriteCommand(Player player) : ICommand
{
    private Player _player = player;
    
    private readonly Vector2 _scale = new(2.0f);
    private readonly int _width = 52;
    private readonly int _height = 56;
    private readonly int _count = 40;
    private readonly float _duration = 0.08f;
    
    public void Execute()
    {
        var sprite = SpriteFactory.Instance.CreateRockingPlayerSprite();
        _player.Sprite = sprite;
        sprite.Scale = _scale;

        _player.Animation = new SpriteAnimation(sprite, _width, _height, _count, _duration);
        _player.IsWalking = false;
    }
}