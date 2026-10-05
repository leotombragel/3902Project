using System;
using System.Collections.Generic;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Entity;

/// <summary>
/// Wizzrobe enemy class, implements IMortal
/// </summary>
public class WizzrobeEnemy : StatefulEntityBase, IMortal
{
    //animation
    private readonly AnimationController _animationController;
    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    //state
    public bool IsFacingLeft { get; private set; }
    public bool IsDead { get; set; }

    //movement
    private static readonly Random _rng = new Random();
    public Vector2 Position { get; private set; }
    public Player _player;
    private int VisibilityBuffer = 0;

    //fireball
    public WizzrobeProjectile fireball;
    private int FireballBuffer = 0;//timer to allow him to become visible AND THEN throw fireball
    public List<IProjectile> _projectiles;
    private bool fireballThrown;

    public WizzrobeEnemy(Player player, List<IProjectile> projectiles)
    {
        Sprite = SpriteFactory.Instance.CreateWizzrobeSprite();
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new WizzrobeAnimationFactory());
        CurrentState = new WizzrobeInvisibleState(this);
        fireballThrown = false;

        _player = player;
        _projectiles = projectiles;

    }
    
    public WizzrobeEnemy(Sprite sprite, Player player, List<IProjectile> projectiles)
    {
        Sprite = sprite;
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new WizzrobeAnimationFactory());
        CurrentState = new WizzrobeInvisibleState(this);
        fireballThrown = false;

        _player = player;
        _projectiles = projectiles;

    }

    //sprite sheet from https://www.spriters-resource.com/nes/legendofzelda/asset/31806/
    public void Draw(SpriteBatch spriteBatch)
    {
        _animationController.Draw(spriteBatch, Position, IsFacingLeft);
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
            ThrowFireball();
        }

        Console.WriteLine(_player.Position);
    
    }

    public bool CheckVisibilityBuffer()
    {
        if (CurrentState is WizzrobeInvisibleState)
        {
            fireballThrown = false;
            if (VisibilityBuffer < 60)//was 240
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
        if(CurrentState is WizzrobeVisibleState)
        {
            int n = _rng.Next(2);// randomize either to the left or right of player
            if (n == 1)
            {
                Position = new Vector2(_player.Position.X - 200, _player.Position.Y + 24);
                IsFacingLeft = false;
            }
            else
            {
                Position = new Vector2(_player.Position.X + 200, _player.Position.Y + 24);
                IsFacingLeft = true;
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
        if (!fireballThrown)
        {
            fireball = new WizzrobeProjectile(Position.X, Position.Y, IsFacingLeft);
            _projectiles.Add(fireball);
            fireballThrown = true;
        }
    }
}