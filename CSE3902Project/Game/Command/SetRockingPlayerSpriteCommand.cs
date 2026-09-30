using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Command;

public class SetRockingPlayerSpriteCommand(Player player) : ICommand
{
    private readonly int _count = 40;
    private readonly float _duration = 0.08f;
    private readonly int _height = 56;

    private readonly Vector2 _scale = new(2.0f);
    private readonly int _width = 52;
    private readonly Player _player = player;

    public void Execute()
    {
        var sprite = SpriteFactory.Instance.CreateRockingPlayerSprite();
        sprite.Scale = _scale;

        _player.Animation = new SpriteAnimation(sprite, _width, _height, _count, _duration);
        _player.IsWalking = false;
    }
}