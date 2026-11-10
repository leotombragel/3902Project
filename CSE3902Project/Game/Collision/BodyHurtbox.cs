using CSE3902Project.Game.Damage;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Collision;

/// <summary>
///     Simple implementation of IHurtbox.
/// </summary>
public sealed class BodyHurtbox : IHurtbox
{
    private readonly Point _offset;
    private readonly Point _size;
    private bool _isEnabled = true;

    public BodyHurtbox(
        ICombatActor owner,
        Point offset,
        Point size)
    {
        Owner = owner;
        _offset = offset;
        _size = size;
    }

    public ICombatActor Owner { get; }

    public bool IsEnabled
    {
        get => _isEnabled && !Owner.IsDead;
        set => _isEnabled = value;
    }

    public Rectangle Bounds => new(
        (int)Owner.Position.X + _offset.X,
        (int)Owner.Position.Y + _offset.Y,
        _size.X,
        _size.Y);
}