using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Graphics;

public interface IAnimatable
{
    bool IsFacingLeft { get; }
    IState CurrentState { get; }
}