using System;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;
using System.Runtime.CompilerServices;

namespace CSE3902Project.Game.Entity;

public class BoomerangProjectile : IProjectile
{
    private const int ThrowSpeed = 5;
    private const int MaxOutboundDistance = 300;

    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    public bool IsReturning = false;
    public bool IsFacingLeft { get; private set; }
    public int XPosition { get; private set; }
    public int XDisplacement {get; private set; }
    public int YPosition { get; private set; }
    public Vector2 Velocity { get; private set; }

    public BoomerangProjectile(int startingX, int startingY, bool isFacingLeft)
    {
        XPosition = isFacingLeft ? startingX : startingX + 32;
        YPosition = startingY + 32;
        IsFacingLeft = isFacingLeft;
        Velocity = new Vector2(isFacingLeft ? -ThrowSpeed : ThrowSpeed, 0.0f);

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