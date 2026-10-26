using System.Collections.Generic;
using System.Linq;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using CSE3902Project.Game.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Collision;

/// <summary>
///     This class handles collision between all objects in the game.
/// </summary>
/// <param name="graphicsDevice">Used for creating rectangles to draw boxes when debugging.</param>
public sealed class CollisionSystem
{
    private readonly Texture2D _debugPixel;
    private readonly Texture2D _debugPixel2;

    public CollisionSystem(GraphicsDevice graphicsDevice)
    {
        _debugPixel = new Texture2D(graphicsDevice, 1, 1);
        _debugPixel.SetData([Color.DarkKhaki * 0.8f]);
        _debugPixel2 = new Texture2D(graphicsDevice, 1, 1);
        _debugPixel2.SetData([Color.DarkGreen * 0.8f]);
    }

    private static bool CanDamage(IDamageSource source, ICombatActor victim)
    {
        if (source.Owner == victim) return false;
        if (source.Owner.Team == victim.Team) return false;
        if (victim.IsDead) return false;
        if (source.HasHit(victim)) return false;
        if (victim.Team == DamageTeam.Item) return false;

        return true;
    }

    /// <summary>
    ///     Checks for collision between actors (hurtboxes) and damage sources (hitboxes), applying damage when
    ///     collision is detected.
    /// </summary>
    /// <param name="actors"></param>
    /// <param name="damageSources"></param>
    public void Update(IReadOnlyList<ICombatActor> actors, IReadOnlyList<IDamageSource> damageSources,
        IReadOnlyList<Rectangle> tileBounds)
    {
        // Collision between hurtboxes and hitboxes
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

        // Collision between item users and items
        var itemUsers = actors.Where(actor => actor is IUsesItems);
        var items = actors.Where(actor => actor is IItem);

        foreach (var source in itemUsers)
        {
            foreach (var target in items)
            {
                if (target.Hurtboxes == null) continue;

                foreach (var itemHurtbox in target.Hurtboxes)
                {
                    if (!itemHurtbox.IsEnabled) continue;

                    foreach (var sourceHurtbox in source.Hurtboxes)
                    {
                        if (!sourceHurtbox.IsEnabled) continue;

                        if (!sourceHurtbox.Bounds.Intersects(itemHurtbox.Bounds)) continue;
                        ((IUsesItems)source).ChangeItem((IItem)target);
                        ((IItem)target).IsDead = true;

                        //collision between enemies
                        foreach (var enemy1 in actors)
                        {
                            if (enemy1 is not IEnemy)
                            {
                                continue;
                            }

                            foreach (var enemy2 in actors)
                            {
                                if (enemy2 is not IEnemy || enemy1.Equals(enemy2))
                                {
                                    continue;
                                }

                                bool collides = false;

                                if (enemy1.Hurtboxes == null || enemy2.Hurtboxes == null)
                                {
                                    continue;
                                }

                                foreach (var hurtbox1 in enemy1.Hurtboxes)
                                {
                                    foreach (var hurtbox2 in enemy2.Hurtboxes)
                                    {
                                        if (hurtbox1.Bounds.Intersects(hurtbox2.Bounds))
                                            collides = true;
                                    }
                                }

                                if (collides)
                                {
                                    if (enemy1.IsFacingLeft != enemy2.IsFacingLeft)
                                    {
                                        enemy1.flipDirection();
                                        enemy2.flipDirection();
                                    }
                                    else if (enemy1.IsFacingLeft)
                                    {
                                        enemy2.flipDirection();
                                    }
                                    else
                                        enemy1.flipDirection();
                                }

                            }
                        }
                    }
                }
            }
        }

        // Collision between projectiles and tiles
        foreach (var Source in damageSources)
        {
            if (!source.IsCollisionEnabled || source.IsFinished || source is not IProjectile) continue;

            foreach (var tile in tileBounds)
                if (source.Bounds.Intersects(tile))
                    source.OnHitTile();
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

                var destination = new Rectangle(hurtbox.Bounds.X, hurtbox.Bounds.Y, hurtbox.Bounds.Width,
                    hurtbox.Bounds.Height);
                spriteBatch.Draw(_debugPixel2, destination, Color.DarkGreen * 0.8f);
            }
        }
    }
}