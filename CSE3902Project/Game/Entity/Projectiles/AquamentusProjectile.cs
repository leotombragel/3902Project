using System;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;
using System.Runtime.CompilerServices;

namespace CSE3902Project.Game.Entity;

public class AquamentusProjectile : IProjectile
{
    private const int ThrowSpeed = 2;
    private const int MaxOutboundDistance = 3000;

    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    public bool IsFacingLeft { get; private set; }
    public bool IsFinished{  get; private set; }
    public float XPosition { get; private set; }
    public float XDisplacement {get; private set; }
    public float YDisplacement {get; private set; }
    public float TotalDisplacement {get; private set; }
    public float YPosition { get; private set; }
    public Vector2 Velocity { get; private set; }

    public AquamentusProjectile(float startingX, float startingY, bool isFacingLeft, double angle)
    {
        XPosition = isFacingLeft ? startingX +40 : startingX + 40;
        YPosition = startingY + 16;
        float dir = isFacingLeft ? 1f : -1f;
        Velocity = new Vector2(dir * (float)(Math.Cos(angle) * ThrowSpeed), (float)(Math.Sin(angle) * ThrowSpeed));

        System.Console.WriteLine($"Velocity: {Velocity.X}, {Velocity.Y}");
        Sprite = SpriteFactory.Instance.CreateAquamentusProjectileSprite();
        Animation = new SpriteAnimation(
            Sprite,
            8,
            10,
            2,
            0.12f,
            231,
            62,
            1);
    }

    private void MoveOutBound()
    {

        var distanceTravelled = Math.Abs(XDisplacement * XDisplacement + YDisplacement * YDisplacement);
        var distanceToMoveX = Math.Min((float)Math.Abs(Velocity.X), MaxOutboundDistance - distanceTravelled);
        var distanceToMoveY = Math.Min((float)Math.Abs(Velocity.Y), MaxOutboundDistance - distanceTravelled);
        var direction = Math.Sign(Velocity.X);

        XPosition += Velocity.X;
        YPosition += Velocity.Y;
        XDisplacement += Velocity.X;
        YDisplacement += Velocity.Y;

    }

    public void Update(GameTime gameTime)
    {
        Animation?.Update(gameTime);
        MoveOutBound();

        TotalDisplacement = (float)Math.Sqrt(XDisplacement * XDisplacement + YDisplacement * YDisplacement);
        if (TotalDisplacement > 400)
        {
            IsFinished = true;
        }
    }
    
    public void Draw(SpriteBatch s)
    {
        Animation?.Draw(s, new Vector2(XPosition, YPosition), !IsFacingLeft);
    }

    
}