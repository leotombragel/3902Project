using System.Collections.Generic;
using System.Linq;
using CSE3902Project.Game.Damage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Collision;

/// <summary>
///    This class handles collision between all objects in the game.
/// </summary>
/// <param name="graphicsDevice">Used for creating rectangles to draw boxes when debugging.</param>
public sealed class CollisionSystem(GraphicsDevice graphicsDevice)
{
    private readonly Texture2D _debugPixel = new(graphicsDevice, 1, 1);
    private readonly Texture2D _debugPixel2 = new(graphicsDevice, 1, 1);

    private static bool CanDamage(IDamageSource source, ICombatActor victim)
    {
        if (source.Owner == victim) return false;
        if (source.Owner.Team == victim.Team) return false;
        if (victim.IsDead) return false;
        if (source.HasHit(victim)) return false;

        return true;
    }

    /// <summary>
    ///     Checks for collision between actors (hurtboxes) and damage sources (hitboxes), applying damage when
    ///     collision is detected.
    /// </summary>
    /// <param name="actors"></param>
    /// <param name="damageSources"></param>
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

    /// <summary>
    ///     Draws hitbox and hurtbox collision bounds for debugging purposes.
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <param name="actors"></param>
    /// <param name="damageSources"></param>
    public void Draw(SpriteBatch spriteBatch, IReadOnlyList<ICombatActor> actors,
        IReadOnlyList<IDamageSource> damageSources)
    {
        foreach (var source in damageSources)
        {
            if (!source.IsCollisionEnabled || source.IsFinished) continue;

            // The alpha layer does not work for this pixel for some reason even though it does for the other pixel.
            // I don't understand why, but it doesn't really matter since this is just for debugging.
            _debugPixel.SetData([Color.DarkKhaki * 0.8f]);
            var destination =
                new Rectangle(source.Bounds.X, source.Bounds.Y, source.Bounds.Width, source.Bounds.Height);
            spriteBatch.Draw(_debugPixel, destination, Color.DarkKhaki * 0.8f);
        }

        foreach (var target in actors)
        {
            if (target.Hurtboxes == null) continue; // Temporary until all entities have hurtboxes

            foreach (var hurtbox in target.Hurtboxes)
            {
                if (!hurtbox.IsEnabled) continue;

                _debugPixel2.SetData([Color.DarkGreen * 0.8f]);
                var destination = new Rectangle(hurtbox.Bounds.X, hurtbox.Bounds.Y, hurtbox.Bounds.Width,
                    hurtbox.Bounds.Height);
                spriteBatch.Draw(_debugPixel2, destination, Color.DarkGreen * 0.8f);
            }
        }
    }
}