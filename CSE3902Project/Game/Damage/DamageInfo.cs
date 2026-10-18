using CSE3902Project.Game.Damage;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Damage;

public sealed record DamageInfo(object Source, int Amount, Vector2 Knockback, DamageTeam Team);