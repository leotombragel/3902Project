using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// <summary>
/// Interface for objects and entities that can be animated.
/// </summary>
public interface IAnimatable
{
    bool IsFacingLeft { get; }
    IState CurrentState { get; }

    public void Draw(SpriteBatch s);

    public void Update(GameTime gameTime);

    public void MoveVertical();

    public void MoveHorizontal(bool isToTheRight);

    public void ChangeState(IState newState);
}