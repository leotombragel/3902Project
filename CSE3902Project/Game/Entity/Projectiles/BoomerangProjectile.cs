using System;
using System.Collections.Generic;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;
using System.Runtime.CompilerServices;
using CSE3902Project.Game.Damage;

namespace CSE3902Project.Game.Entity;

public class BoomerangProjectile : IProjectile
{
    private const int ThrowSpeed = 5;
    private const int MaxOutboundDistance = 300;
    private Vector2 PlayerCenterCorrection = new Vector2(32, 16);

    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    public Player _player { get; }
    private readonly HashSet<ICombatActor> _hitTargets = new();
    
    public bool IsReturning = false;
    public bool IsFinished { get; private set; }
    public bool IsFacingLeft { get; private set; }
    public ICombatActor Owner => _player;
    public DamageInfo Damage { get; }
    public Rectangle Bounds => new(XPosition, YPosition, 24, 24);
    public bool IsCollisionEnabled => !IsFinished;
    public int XPosition { get; private set; }
    public int XDisplacement {get; private set; }
    public int YPosition { get; private set; }
    public Vector2 Velocity { get; private set; }

    public bool HasHit(ICombatActor victim)
    {
        return _hitTargets.Contains(victim);
    }

    public void RecordHit(ICombatActor victim)
    {
        _hitTargets.Add(victim);
    }

    public void OnHit(ICombatActor victim)
    {
        // The boomerang continues moving after a hit.
    }

    public void OnHitTile()
    {
        // The boomerang bounces off a tile.
        IsReturning = true;
    }

    public BoomerangProjectile(Player player, int startingX, int startingY, bool isFacingLeft)
    {
        _player = player;
        XPosition = isFacingLeft ? startingX : startingX + 32;
        YPosition = startingY + 32;
        IsFacingLeft = isFacingLeft;
        Velocity = new Vector2(isFacingLeft ? -ThrowSpeed : ThrowSpeed, 0.0f);
        Damage = new DamageInfo(this, 1, Vector2.Zero, DamageTeam.Player);

        Sprite = SpriteFactory.Instance.CreateItemsSheet();
        Animation = new SpriteAnimation(
            Sprite,
            12,
            12,
            4,
            0.12f,
            126,
            85,
            12);
    }

    private void MoveOutBound()
    {
        if (IsReturning) return;

        var distanceTravelled = Math.Abs(XDisplacement);
        var distanceToMove = Math.Min((int)Math.Abs(Velocity.X), MaxOutboundDistance - distanceTravelled);
        var direction = Math.Sign(Velocity.X);

        XPosition += direction * distanceToMove;
        XDisplacement += direction * distanceToMove;

        if (Math.Abs(XDisplacement) >= MaxOutboundDistance)
        {
            IsReturning = true;
        }
    }

    private void MoveInBound()
    {
        if (!IsReturning) return;

        Vector2 playerPos = _player.Position;
        float xDisplacement = playerPos.X + 16 - XPosition;
        float yDisplacement = playerPos.Y + 32 - YPosition;
        float totalDisplacement = MathF.Sqrt(
            xDisplacement * xDisplacement + yDisplacement * yDisplacement);

        if (totalDisplacement <= ThrowSpeed)
        {
            XPosition = (int)MathF.Round(playerPos.X + 16);
            YPosition = (int)MathF.Round(playerPos.Y + 32);
            IsFinished = true;
            return;
        }

        float angle = MathF.Atan2(yDisplacement, xDisplacement);
        XPosition += (int)MathF.Round(MathF.Cos(angle) * ThrowSpeed);
        YPosition += (int)MathF.Round(MathF.Sin(angle) * ThrowSpeed);
    }

    public void Update(GameTime gameTime)
    {
        Animation?.Update(gameTime);

        if (!IsReturning)
        {
            MoveOutBound();
        } else {
            MoveInBound();
        }
    }
    
    public void Draw(SpriteBatch s)
    {
        Animation?.Draw(s, new Vector2(XPosition, YPosition), !IsFacingLeft);
    }

    
}