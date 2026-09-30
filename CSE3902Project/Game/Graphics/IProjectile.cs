using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// <summary>
/// Interface for objects and entities that can be animated.
/// </summary>
public interface IProjectile
{

    bool IsFinished { get; }

    SpriteAnimation Animation { get; set;  }

    Vector2 Velocity { get; }

    public void Draw(SpriteBatch s);

    public void Update(GameTime gameTime);

}