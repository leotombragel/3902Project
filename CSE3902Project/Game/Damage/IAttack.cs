using CSE3902Project.Game.Collision;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Damage;

public interface IAttack : IDamageSource
{
    void Update(GameTime gameTime);
}