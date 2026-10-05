using System;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Entity;

/// <summary>
/// keese enemy class, implements IMortal
/// </summary>
public class KeeseEnemy : IMortal
{
    //animation
    private readonly AnimationController _animationController;
    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    //state
    public bool IsFacingLeft { get; private set; }
    public bool IsGoingUp { get; private set; }
    public IState CurrentState { get; private set;}
    public bool IsDead { get; set; } // for list culling later

    //movement
    private static readonly Random _rng = new Random();
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }
    public Vector2 BoxContainer { get; private set; } //constricting values so keese doesn't fly off the screen
    private const int StartingPosX = 400; 
    private const int StartingPosY = 100; 
    
    public KeeseEnemy()
    {
        Sprite = SpriteFactory.Instance.CreateKeeseSprite();
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = new Vector2(-1, 1);
        BoxContainer = new Vector2(100, 50);
        IsGoingUp = false;
        IsFacingLeft = true;
            _animationController = new AnimationController(this, new KeeseAnimationFactory());
        CurrentState = new KeeseFlyingState(this);
    }
    
    public KeeseEnemy(Sprite sprite)
    {
        Sprite = sprite;
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = new Vector2(-1, 1);
        BoxContainer = new Vector2(100, 50);
        IsGoingUp = false;
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new KeeseAnimationFactory());
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

        IsGoingUp = MoveVertical(IsGoingUp);
        IsFacingLeft = MoveHorizontal(IsFacingLeft);
        
        UpdatePosition();
    }

    public bool MoveVertical(bool IsGoingUp)
    {
        var temp = Velocity.Y;
        IsGoingUp = CheckVerticalBoundary(IsGoingUp);
        if(temp != Velocity.Y)
        {
            return IsGoingUp;
        }

        int n = _rng.Next(60);
        if(n == 0)
        {
            IsGoingUp = FlipVertical(IsGoingUp);
        }
        return IsGoingUp;
    }

    public bool CheckVerticalBoundary(bool IsGoingUp)
    {
        if(Position.Y  < StartingPosY - BoxContainer.Y)
        {
            IsGoingUp = false;
            Velocity = Velocity with {Y = -Velocity.Y};
        }
        else if(Position.Y  > StartingPosY + BoxContainer.Y)
        {
            IsGoingUp = true;
            Velocity = Velocity with {Y = -Velocity.Y};
        }
        return IsGoingUp;
    }

    public bool FlipVertical(bool IsGoingUp)
    {
        if (IsGoingUp)
        {
            IsGoingUp = false;
            if (Velocity.Y < 0)
            {
                Velocity = Velocity with {Y = -Velocity.Y};
            }
        }
        else
        {
            IsGoingUp = true;
            if (Velocity.Y > 0)
            {
                Velocity = Velocity with {Y = -Velocity.Y};
            }
        }
        return IsGoingUp;
    }

    public bool MoveHorizontal(bool IsFacingLeft)
    {
        var temp = Velocity.X;
        IsFacingLeft = CheckHorizontalBoundary(IsFacingLeft);
        if(temp != Velocity.X)
        {
            return IsFacingLeft;
        }

        int n = _rng.Next(60);
        if(n == 0)
        {
            IsFacingLeft = FlipHorizontal(IsFacingLeft);
        }
        return IsFacingLeft;
    }
    
    public bool CheckHorizontalBoundary(bool IsFacingLeft)
    {
        if(Position.X  < StartingPosX - BoxContainer.X)
        {
            IsFacingLeft= false;
            Velocity = Velocity with {X = -Velocity.X};
        }
        else if(Position.X  > StartingPosX + BoxContainer.X)
        {
            IsFacingLeft = true;
            Velocity = Velocity with {X = -Velocity.X};
        }
        return IsFacingLeft;
    }

    public bool FlipHorizontal(bool IsFacingLeft)
    {
        if (IsFacingLeft)
        {
            IsFacingLeft = false;
            if(Velocity.X < 0 )
            {
                Velocity = Velocity with {X = -Velocity.X};
            }
        }
        else
        {
            IsFacingLeft = true;
            if(Velocity.X > 0 )
            {
                Velocity = Velocity with {X = -Velocity.X};
            }
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