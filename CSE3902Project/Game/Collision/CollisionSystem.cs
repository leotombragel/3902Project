using System;
using System.Collections.Generic;
using System.Linq;
using CSE3902Project.Game.Damage;

namespace CSE3902Project.Game.Collision;

public sealed class CollisionSystem
{
    private static bool CanDamage(IDamageSource source, ICombatActor victim)
    {
        if (source.Owner == victim) return false;
        if (source.Owner.Team == victim.Team) return false;
        if (victim.IsDead) return false;
        if (source.HasHit(victim)) return false;

        return true;
    }

    public void Update(IReadOnlyList<ICombatActor> actors, IReadOnlyList<IDamageSource> damageSources)
    {
        foreach (var source in damageSources)
        {
            if (!source.IsCollisionEnabled || source.IsFinished) continue;

            foreach (var target in actors)
            {
                if (!CanDamage(source, target)) continue;
                if (target.Hurtboxes == null) continue; // Temporary until all entities have hurtboxes

                if (target.Hurtboxes.Where(hurtbox => hurtbox.IsEnabled)
                    .Any(hurtbox => source.Bounds.Intersects(hurtbox.Bounds)))
                {
                    source.RecordHit(target);
                    target.TakeDamage(source.Damage);
                    source.OnHit(target);
                }
            }
        }
    }
}