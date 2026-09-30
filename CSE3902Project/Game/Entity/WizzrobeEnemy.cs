using System;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Entity;

/// <summary>
/// Wizzrobe enemy class, implements IAnimatable
/// </summary>
public class WizzrobeEnemy : IAnimatable
{
    //animation
    private readonly AnimationController _animationController;
    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    //state
    public bool IsFacingLeft { get; private set; }
    public IState CurrentState { get; private set;}

    //movement
    private static readonly Random _rng = new Random();
    public Vector2 Position { get; private set; }
    public Vector2 PlayerPos;
    public int VisibilityBuffer = 0;

    public WizzrobeEnemy()
    {
        Sprite = SpriteFactory.Instance.CreateWizzrobeSprite();
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new PlaceholderAnimFactory());
        CurrentState = new WizzrobeInvisibleState(this);
    }
    
    public WizzrobeEnemy(Sprite sprite)
    {
        Sprite = sprite;
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new PlaceholderAnimFactory());
        CurrentState = new WizzrobeInvisibleState(this);
    }

    //sprite sheet from https://www.spriters-resource.com/nes/legendofzelda/asset/31806/
    public void Draw(SpriteBatch spriteBatch)
    {
        _animationController.Draw(spriteBatch, Position, IsFacingLeft);
    }
    
    public void setPlayerPos(Vector2 pos)
    {
        PlayerPos = pos;
    }

    public virtual void Update(GameTime gameTime)
    {
        _animationController.Update(gameTime);
        Animation?.Update(gameTime);
        CurrentState.Update(gameTime);

        if(checkVisibilityBuffer())
        {
            UpdatePosition();
        }
    
    }

    public bool checkVisibilityBuffer()
    {
        Console.WriteLine("wizzrobe state is " + CurrentState);
        if (CurrentState is WizzrobeInvisibleState)
        {
            if (VisibilityBuffer < 240)
            {
                VisibilityBuffer += 1;
            }
            else
            {
                VisibilityBuffer = 0;
                ChangeState(new WizzrobeVisibleState(this));
                return true;
            }
        }
        else if(CurrentState is WizzrobeVisibleState)
        {
            if (VisibilityBuffer < 300)
            {
                VisibilityBuffer += 1;
            }
            else
            {
                VisibilityBuffer = 0;
                ChangeState(new WizzrobeInvisibleState(this));
                return true;
            }
        }
        return false;
    }

    private void UpdatePosition()
    {
        if(CurrentState is WizzrobeInvisibleState)
        {
            Position = new Vector2(0, -20);//put him off screen
        }
        else if(CurrentState is WizzrobeVisibleState)
        {
            int n = _rng.Next(2);// randomize either to the left or right of player
            if (n == 1)
            {
                Position = new Vector2(PlayerPos.X -= 200, PlayerPos.Y + 24);
                IsFacingLeft = true;
            }
            else
            {
                Position = new Vector2(PlayerPos.X += 200, PlayerPos.Y + 24);
                IsFacingLeft = false;
            }
        }
    }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
}