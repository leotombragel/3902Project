using System;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;
using System.Runtime.CompilerServices;

namespace CSE3902Project.Game.Entity;

public class BombProjectile : IProjectile
{
    private const float FuseDuration = 2.0f;
    private const float ExplosionFrameDuration = 0.12f;
    private const int FrameWidth = 21;
    private const int FrameHeight = 21;
    private const int FrameStartX = 2;
    private const int FrameStartY = 104;
    private const int FrameBufferWidth = 3;
    private const int BombIdleFrame = 4;

    private const int ThrowSpeed = 5;

    private float _fuseTimer;

    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    public bool IsReturning = false;
    public bool IsExploding { get; private set; }
    public bool IsFinished { get; private set; }
    public bool IsFacingLeft { get; private set; }
    public int XPosition { get; private set; }
    public int XDisplacement {get; private set; }
    public int YPosition { get; private set; }
    public Vector2 Velocity { get; private set; }

    public BombProjectile(int startingX, int startingY, bool isFacingLeft)
    {
        XPosition = isFacingLeft ? startingX : startingX + 32;
        YPosition = startingY + 11;
        IsFacingLeft = isFacingLeft;
        Velocity = new Vector2(isFacingLeft ? -ThrowSpeed : ThrowSpeed, 0.0f);

        Sprite = SpriteFactory.Instance.CreateItemsSheet();
        Sprite.SourceRectangle = GetFrameRectangle(BombIdleFrame);
    }

    private Rectangle GetFrameRectangle(int frameIndex)
    {
        var frameStride = FrameWidth + FrameBufferWidth;
        return new Rectangle(
            FrameStartX + frameIndex * frameStride,
            FrameStartY,
            FrameWidth,
            FrameHeight);
    }

    public void Update(GameTime gameTime)
    {
        if (!IsExploding)
        {
            _fuseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_fuseTimer < FuseDuration) return;

            IsExploding = true;
            Animation = new SpriteAnimation(
                Sprite,
                FrameWidth,
                FrameHeight,
                new[] { 2, 1, 0, 3 },
                0.12f,
                FrameStartX,
                FrameStartY,
                FrameBufferWidth);
            return;
        }

        Animation?.Update(gameTime);
        if (Animation?.LoopCount > 0)
        {
            IsFinished = true;
        }
    }
    
    public void Draw(SpriteBatch s)
    {
        var position = new Vector2(XPosition, YPosition);
        if (IsExploding)
        {
            Animation?.Draw(s, position, IsFacingLeft);
        }
        else
        {
            Sprite.Draw(s, position, IsFacingLeft);
        }
    }

    
}