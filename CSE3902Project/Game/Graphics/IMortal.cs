using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// <summary>
/// Interface for objects and entities that can be animated.
/// </summary>
public interface IMortal : IAnimatable
{
    bool IsFacingLeft { get; }

    bool IsDead { get; set;  }
    IState CurrentState { get; }
    SpriteAnimation Animation { get; set;  }

    public void Draw(SpriteBatch s);

    public void Update(GameTime gameTime);

    public void ChangeState(IState newState);
}