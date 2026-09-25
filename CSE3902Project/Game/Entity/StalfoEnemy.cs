using System;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Entity;

/// <summary>
/// stalfo enemy class, implements IAnimatable
/// </summary>
public class StalfoEnemy : IAnimatable
{
    //animation
    private readonly AnimationController _animationController;
    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }
    public int FrameBuffer = 0;//Stalfo has no animation, just a frame flipped over the y axis

    //state
    public bool IsFacingLeft { get; private set; }
    public IState CurrentState { get; private set;}

    //movement
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }
    private const float VerticalMoveSpeed = 4.0f;
    private const float MaxSpeed = 3.0f;
    private const int StartingPosX = 600; //figure out how to set these through constructor late
    private const int StartingPosY = 100; 

        public StalfoEnemy()
    {
        Sprite = SpriteFactory.Instance.CreateIdlePlayerSprite();
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = Vector2.Zero;
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new PlaceholderAnimFactory());
        CurrentState = new StalfoLeftState(this);
    }
    
    public StalfoEnemy(Sprite sprite)
    {
        Sprite = sprite;
        Velocity = Vector2.Zero;
        Position = new Vector2(StartingPosX, StartingPosY);
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new PlaceholderAnimFactory());
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
    /// Uses the velocity to update the keese's position.
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
        
        UpdatePosition();
    }

    public void MoveVertical()
    {
        // _verticalSpeed = -VerticalMoveSpeed; 
        Velocity = Velocity with { Y = -VerticalMoveSpeed };
    }

    public void MoveHorizontal(bool isToTheRight)
    {

    }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
}