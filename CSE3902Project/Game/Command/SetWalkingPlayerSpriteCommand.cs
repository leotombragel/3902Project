using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Command;

public class SetWalkingPlayerSpriteCommand(Player player) : ICommand
{
    private readonly int _count = 16;
    private readonly float _duration = 0.08f;
    private readonly int _height = 44;

    // Configuration to make the sprite look correct
    private readonly Vector2 _scale = new(2.0f);
    private readonly int _width = 39;
    private readonly Player _player = player;

    public void Execute()
    {
        if (_player.IsWalking) return;
        var sprite = SpriteFactory.Instance.CreateWalkingPlayerSprite();
        sprite.Scale = _scale;

        _player.Animation = new SpriteAnimation(sprite, _width, _height, _count, _duration);

        _player.IsWalking = true;
    }
}