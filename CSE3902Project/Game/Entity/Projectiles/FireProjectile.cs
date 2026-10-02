using System;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;
using System.Runtime.CompilerServices;

namespace CSE3902Project.Game.Entity;

public class FireProjectile : IProjectile
{

    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    private const int ThrowSpeed = 5;
    private const int MaxOutboundDistance = 300;

    public bool IsFinished { get; private set; }
    public bool IsFacingLeft { get; private set; }
    public int XPosition { get; private set; }
    public int XDisplacement {get; private set; }
    public int YPosition { get; private set; }
    public Vector2 Velocity { get; private set; }

    public FireProjectile(int startingX, int startingY, bool isFacingLeft)
    {

        XPosition = isFacingLeft ? startingX : startingX + 32;
        YPosition = startingY + 16;
        IsFacingLeft = isFacingLeft;
        Velocity = new Vector2(isFacingLeft ? -ThrowSpeed : ThrowSpeed, 0.0f);

        Sprite = SpriteFactory.Instance.CreateFireSprite();
        Animation = new SpriteAnimation(
            Sprite,
            16,
            24,
            3,
            0.12f,
            180,
            49,
            8);
    }

    public void Update(GameTime gameTime)
    {
        Animation?.Update(gameTime);

        var distanceTravelled = Math.Abs(XDisplacement);
        var distanceToMove = Math.Min((int)Math.Abs(Velocity.X), MaxOutboundDistance - distanceTravelled);
        var direction = Math.Sign(Velocity.X);

        XPosition += direction * distanceToMove;
        XDisplacement += direction * distanceToMove;

        if (Math.Abs(XDisplacement) >= MaxOutboundDistance)
        {
            IsFinished = true;
        }
    }
    
    public void Draw(SpriteBatch s)
    {
        Animation?.Draw(s, new Vector2(XPosition, YPosition), !IsFacingLeft);
    }

    
}