using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Graphics;
using CSE3902Project.Game.Collision;

namespace CSE3902Project.Game.Entity;

public interface IEnemy : ICombatActor, IAnimatable
{
    IHurtbox _bodyHurtbox { get; }
}
