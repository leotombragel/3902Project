using System;
using System.Collections.Generic;
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
    private int VisibilityBuffer = 0;

    //fireball
    public WizzrobeProjectile fireball;
    private int FireballBuffer = 0;//timer to allow him to become visible AND THEN throw fireball
    public List<IProjectile> _projectiles;

    public WizzrobeEnemy(Vector2 playerPos, List<IProjectile> projectiles)
    {
        Sprite = SpriteFactory.Instance.CreateWizzrobeSprite();
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new WizzrobeAnimationFactory());
        CurrentState = new WizzrobeInvisibleState(this);

        PlayerPos = playerPos;
        _projectiles = projectiles;

    }
    
    public WizzrobeEnemy(Sprite sprite)
    {
        Sprite = sprite;
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new WizzrobeAnimationFactory());
        CurrentState = new WizzrobeInvisibleState(this);
    }

    //sprite sheet from https://www.spriters-resource.com/nes/legendofzelda/asset/31806/
    public void Draw(SpriteBatch spriteBatch)
    {
        _animationController.Draw(spriteBatch, Position, IsFacingLeft);
    }
    
    public void setPlayerPos(Vector2 pos)
    {
        
    }

    public virtual void Update(GameTime gameTime)
    {
        _animationController.Update(gameTime);
        Animation?.Update(gameTime);
        CurrentState.Update(gameTime);

        if(CheckVisibilityBuffer())
        {
            UpdatePosition();
        }

        if(CheckFireballBuffer())
        {
            Console.WriteLine("thrown");
            ThrowFireball();
        }
    
    }

    public bool CheckVisibilityBuffer()
    {
        if (CurrentState is WizzrobeInvisibleState)
        {
            if (VisibilityBuffer < 24)//was 240
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

    private bool CheckFireballBuffer()
    {
        if(CurrentState is WizzrobeVisibleState)
        {
            if (FireballBuffer < 20)
            {
                FireballBuffer += 1;
            }
            else
            {
                FireballBuffer = 0;
                return true;
            }
        }
        return false;
    }

    private void ThrowFireball()
    {
        fireball = new WizzrobeProjectile(Position.X, Position.Y, IsFacingLeft);
        _projectiles.Add(fireball);
    }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
}