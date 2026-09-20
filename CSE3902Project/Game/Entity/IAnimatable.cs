using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Entity;

public interface IAnimatable
{
    Direction Facing { get; }
    IState CurrentState { get; }
}