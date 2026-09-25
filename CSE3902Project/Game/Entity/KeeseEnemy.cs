using System;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Entity;

/// <summary>
/// keese enemy class, implements IAnimatable
/// </summary>
public class KeeseEnemy : IAnimatable
{
    //animation
    private readonly AnimationController _animationController;
    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    //state
    public bool IsFacingLeft { get; private set; }
    public IState CurrentState { get; private set;}

    //movement
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }
    private const float VerticalMoveSpeed = 4.0f;
    private const float MaxSpeed = 3.0f;
    private const int StartingPosX = 400; //figure out how to set these through constructor late
    private const int StartingPosY = 100; 

        public KeeseEnemy()
    {
        Sprite = SpriteFactory.Instance.CreateIdlePlayerSprite();
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = Vector2.Zero;
        _animationController = new AnimationController(this, new PlaceholderAnimFactory());
        CurrentState = new KeeseFlyingState(this);
    }
    
    public KeeseEnemy(Sprite sprite)
    {
        Sprite = sprite;
        Velocity = Vector2.Zero;
        Position = new Vector2(StartingPosX, StartingPosY);
        _animationController = new AnimationController(this, new PlaceholderAnimFactory());
        CurrentState = new KeeseStoppedState(this);
    }

    //sprite sheet from https://www.spriters-resource.com/nes/legendofzelda/asset/31806/
    public void Draw(SpriteBatch spriteBatch)
    {
        _animationController.Draw(spriteBatch, Position, IsFacingLeft);
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