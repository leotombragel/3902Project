using System;
using System.Collections.Generic;
using CSE3902Project.Game.Collision;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Entity.State;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Entity;

/// <summary>
///     Wizzrobe enemy class, implements IMortal
/// </summary>
public class WizzrobeEnemy : StatefulEntityBase, IEnemy
{
    //movement
    private static readonly Random _rng = new();

    //animation
    private readonly AnimationController _animationController;
    public Player _player;
    public List<IProjectile> _projectiles;

    //fireball
    public WizzrobeProjectile fireball;
    private int FireballBuffer; //timer to allow him to become visible AND THEN throw fireball
    private bool fireballThrown;
    private int VisibilityBuffer;

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

    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    //state
    public bool IsFacingLeft { get; private set; }
    public bool IsDead { get; set; } // for list culling later

    public void TakeDamage(DamageInfo damageInfo)
    {
        // TODO
        Console.WriteLine($"Wizzrobe took damage. Source: {damageInfo.Source}");
    }

    public Vector2 Position { get; private set; }

    public DamageTeam Team => DamageTeam.Enemy;

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

        if (CheckVisibilityBuffer()) UpdatePosition();

        if (CheckFireballBuffer()) ThrowFireball();
    }

    public IEnumerable<IHurtbox> Hurtboxes { get; }

    public bool CheckVisibilityBuffer()
    {
        if (CurrentState is WizzrobeInvisibleState)
        {
            fireballThrown = false;
            if (VisibilityBuffer < 60)
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
        else if (CurrentState is WizzrobeVisibleState)
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
        if (CurrentState is WizzrobeVisibleState)
        {
            var n = _rng.Next(2); // randomize either to the left or right of player
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
        if (CurrentState is WizzrobeVisibleState)
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
            fireball = new WizzrobeProjectile(Position.X, Position.Y, IsFacingLeft, this);
            _projectiles.Add(fireball);
            fireballThrown = true;
        }
    }
}