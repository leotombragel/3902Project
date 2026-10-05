using System;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Entity;

/// <summary>
/// stalfo enemy class, implements IMortal
/// </summary>
public class StalfoEnemy : IMortal
{
    //animation
    private readonly AnimationController _animationController;
    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }
    public int FrameBuffer = 0;//Stalfo has no animation, just a frame flipped over the y axis

    //state
    public bool IsFacingLeft { get; private set; }
    public IState CurrentState { get; private set;}
    public bool IsDead { get; set; } // for list culling later

    //movement
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }
    private const int StartingPosX = 600; 
    private const int StartingPosY = 100; 
    public int LeftRightBuffer = 0;//use this until we get collision to switch direction like a goomba

    public StalfoEnemy()
    {
        Sprite = SpriteFactory.Instance.CreateStalfoSprite();
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = new Vector2(-1, 0);
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new StalfoAnimationFactory());
        CurrentState = new StalfoLeftState(this);
    }
    
    public StalfoEnemy(Sprite sprite)
    {
        Sprite = sprite;
        Velocity = new Vector2(-1, 0);
        Position = new Vector2(StartingPosX, StartingPosY);
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new StalfoAnimationFactory());
        CurrentState = new StalfoLeftState(this);
    }

    //sprite sheet from https://www.spriters-resource.com/nes/legendofzelda/asset/31806/
    public void Draw(SpriteBatch spriteBatch)
    {
        _animationController.Draw(spriteBatch, Position, IsFacingLeft);
        if (FrameBuffer < 10)
        {
            FrameBuffer += 1;
        }
        else
        {
            if (IsFacingLeft)
            {
                IsFacingLeft = false;
            }
            else
            {
                IsFacingLeft = true;
            }
            FrameBuffer = 0;
        }
    }

    /// <summary>
    /// Uses the velocity to update the stalfo's position.
    /// </summary>
    private void UpdatePosition()
    {
        Position += Velocity;
    }

    public virtual void Update(GameTime gameTime)
    {
        _animationController.Update(gameTime);
        Animation?.Update(gameTime);
        CurrentState.Update(gameTime);

        IsFacingLeft = MoveHorizontal(IsFacingLeft);
        UpdatePosition();
    }

    public bool MoveHorizontal(bool IsFacingLeft)
    {
    //wait to implement further when we get proper collision
    if (LeftRightBuffer < 400)
    {
        LeftRightBuffer += 1;
    }
    else
    {
           LeftRightBuffer = 0;
           IsFacingLeft = !IsFacingLeft;
           Velocity = new Vector2(Velocity.X * -1, Velocity.Y);

    }
    return IsFacingLeft;
    }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
}