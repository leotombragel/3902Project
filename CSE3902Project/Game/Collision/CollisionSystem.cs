using System;
using System.Collections.Generic;
using System.Linq;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using CSE3902Project.Game.Items;
using CSE3902Project.Game.Tiles;
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
    private readonly Texture2D _debugOutlinePixel;

    public CollisionSystem(GraphicsDevice graphicsDevice)
    {
        _debugPixel = new Texture2D(graphicsDevice, 1, 1);
        _debugPixel.SetData([Color.DarkKhaki * 0.8f]);
        _debugPixel2 = new Texture2D(graphicsDevice, 1, 1);
        _debugPixel2.SetData([Color.DarkGreen * 0.8f]);
        _debugOutlinePixel = new Texture2D(graphicsDevice, 1, 1);
        _debugOutlinePixel.SetData([Color.White]);
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
    ///     Checks for collision between player and enemy hurtboxes, specifically when link runs into an enemy.
    ///     applying damage when collision is detected.
    /// </summary>
    /// <param name="actors"></param>
    /// <param name="enemies"></param>
    /// <param name="tileBounds"></param>
    public void Update(Player _player, IReadOnlyList<IEnemy> enemies, IReadOnlyList<Rectangle> tileBounds)
    {
        if (_player.IsDead) return;

        foreach (var enemy in enemies)
        {
            if (!_player._bodyHurtbox.IsEnabled || enemy.IsDead || !enemy._bodyHurtbox.IsEnabled) continue;

            if (enemy._bodyHurtbox.Bounds.Intersects(_player._bodyHurtbox.Bounds))
                _player.TakeDamage(new DamageInfo(this, 1, Vector2.Zero, DamageTeam.Enemy));
        }
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

            if (tileBounds.Any(tile => source.Bounds.Intersects(tile)))
            {
                source.OnHitTile();
                continue;
            }

            foreach (var target in actors)
            {
                if (!CanDamage(source, target)) continue;

                if (target.Hurtboxes.Where(hurtbox => hurtbox.IsEnabled)
                    .Any(hurtbox => source.Bounds.Intersects(hurtbox.Bounds)))
                {
                    source.RecordHit(target);
                    target.TakeDamage(source.Damage);
                    source.OnHit(target);
                    if (source.IsFinished) break;
                }
            }
        }

        // Collision between item users and items
        var itemUsers = actors.Where(actor => actor is IUsesItems);
        var items = actors.Where(actor => actor is IItem);

        foreach (var source in itemUsers)
        foreach (var target in items)
        {
            if (target.Hurtboxes == null) continue;
            if (target.IsDead) continue;

            foreach (var itemHurtbox in target.Hurtboxes)
            {
                if (!itemHurtbox.IsEnabled) continue;

                foreach (var sourceHurtbox in source.Hurtboxes)
                {
                    if (!sourceHurtbox.IsEnabled) continue;

                    if (!sourceHurtbox.Bounds.Intersects(itemHurtbox.Bounds)) continue;
                    ((IUsesItems)source).InteractWithItem((IItem)target);
                    ((IItem)target).IsDead = true;
                }
            }
        }

        // Collision between projectiles and tiles
        foreach (var source in damageSources)
        {
            if (!source.IsCollisionEnabled || source.IsFinished || source is not IProjectile) continue;

            foreach (var tile in tileBounds)
                if (source.Bounds.Intersects(tile))
                    source.OnHitTile();
        }

        //collision between enemies
        var enemies = actors.Where(actor => actor is IEnemy).ToList();
        var seenCombinations = new HashSet<(ICombatActor, ICombatActor)>();
        
        foreach (var enemy1 in enemies)
        {
            if (enemy1.IsDead) continue;

            foreach (var enemy2 in enemies)
            {
                if (enemy2.IsDead) continue;
                if (enemy1.Equals(enemy2)) continue;
                
                var combination = (enemy1, enemy2);
                var reverseCombination = (enemy2, enemy1);
                var isNew = seenCombinations.Add(combination) && seenCombinations.Add(reverseCombination);
                
                if (!isNew) continue;

                var collides = false;

                if (enemy1.Hurtboxes == null || enemy2.Hurtboxes == null) continue;

                foreach (var hurtbox1 in enemy1.Hurtboxes)
                foreach (var hurtbox2 in enemy2.Hurtboxes)
                    if (hurtbox1.Bounds.Intersects(hurtbox2.Bounds))
                        collides = true;

                if (collides)
                {
                    if (enemy1.IsFacingLeft != enemy2.IsFacingLeft)
                    {
                        enemy1.FlipDirection();
                        enemy2.FlipDirection();
                    }
                    else if (enemy1.IsFacingLeft)
                    {
                        enemy2.FlipDirection();
                    }
                    else
                    {
                        enemy1.FlipDirection();
                    }
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

    public void DrawStageHitboxes(SpriteBatch spriteBatch, IReadOnlyList<ITile> tiles, Rectangle playerHitbox,
        IReadOnlyList<Rectangle> pipeEntrances, IReadOnlyList<Rectangle> pipeExits)
    {
        foreach (var tile in tiles)
        {
            DrawOutline(spriteBatch, tile.Bounds, Color.Red);
        }

        DrawOutline(spriteBatch, playerHitbox, Color.LimeGreen);
        foreach (var pipeEntrance in pipeEntrances)
        {
            DrawOutline(spriteBatch, pipeEntrance, Color.Yellow);
        }

        foreach (var pipeExit in pipeExits)
        {
            DrawOutline(spriteBatch, pipeExit, Color.DeepSkyBlue);
        }
    }

    private void DrawOutline(SpriteBatch spriteBatch, Rectangle bounds, Color color)
    {
        const int thickness = 1;
        spriteBatch.Draw(_debugOutlinePixel, new Rectangle(bounds.Left, bounds.Top, bounds.Width, thickness), color);
        spriteBatch.Draw(_debugOutlinePixel,
            new Rectangle(bounds.Left, bounds.Bottom - thickness, bounds.Width, thickness), color);
        spriteBatch.Draw(_debugOutlinePixel, new Rectangle(bounds.Left, bounds.Top, thickness, bounds.Height), color);
        spriteBatch.Draw(_debugOutlinePixel,
            new Rectangle(bounds.Right - thickness, bounds.Top, thickness, bounds.Height), color);
    }
}