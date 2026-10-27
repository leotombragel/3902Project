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
///     aquamentus enemy class, implements IEnemy
/// </summary>
public class AquamentusEnemy : IEnemy
{
    //movement
    public Vector2 Position { get; private set; }
    private readonly int StartingPosY = 320;
    private const int StartingPosX = 600;
    public IHurtbox _bodyHurtbox { get; }

    //animation
    private readonly AnimationController _animationController;
    private const int SpriteFrameHeight = 32;
    private const int SpriteScale = 3;

    // references
    private readonly Player _player;
    private readonly List<IProjectile> _projectiles;
    public int health { get; set; }

    //projectile
    public AquamentusProjectile fireball1, fireball2, fireball3;
    private int FireballsBuffer; //timer to allow him to become visible AND THEN throw fireball
    public int FrameBuffer = 0;
    public int LeftRightBuffer; 
    public Sprite Sprite { get; set; }
    public Vector2 Velocity { get; private set; }
    public SpriteAnimation Animation { get; set; }

    //state
    public bool IsFacingLeft { get; private set; }
    public IState CurrentState { get; private set; }
    public bool IsDead { get; set; }
    private int deadAnimationBuffer = 0;

    public AquamentusEnemy(Player player, List<IProjectile> projectiles)
    {
        _player = player;
        _projectiles = projectiles;
        Sprite = SpriteFactory.Instance.CreateAquamentusSprite();
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = new Vector2(-1, 0);
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new AquamentusAnimationFactory());
        CurrentState = new AquamentusLeftState(this);
        health = 2;
        _bodyHurtbox = new BodyHurtbox(this, new Point(0, 0), new Point(96, 128));
    }

    public AquamentusEnemy(Player player, List<IProjectile> projectiles, Vector2 spawnPosition)
        : this(player, projectiles)
    {
        Position = spawnPosition - new Vector2(0, SpriteFrameHeight * SpriteScale);
    }

    // Damage
    public void TakeDamage(DamageInfo damageInfo)
    {
        // _mortalImplementation.TakeDamage(damageInfo);
        ChangeState(new AquamentusDeadState(this));
    }

    public DamageTeam Team => DamageTeam.Enemy;

    public IEnumerable<IHurtbox> Hurtboxes
    {
        get { yield return _bodyHurtbox; }
    }

    //sprite sheet from https://www.spriters-resource.com/nes/legendofzelda/asset/31806/
    public void Draw(SpriteBatch spriteBatch)
    {
        _animationController.Draw(spriteBatch, Position, IsFacingLeft);
    }

    public virtual void Update(GameTime gameTime)
    {
        if (CurrentState is not AquamentusDeadState){
            _animationController.Update(gameTime);
            Animation?.Update(gameTime);
            CurrentState.Update(gameTime);

            if (CheckFireballBuffer()) ThrowFireball();
            UpdateDirection();
            MoveHorizontal(IsFacingLeft);
            UpdatePosition();
        }
        else{
            if (deadAnimationBuffer < 16)
            {
                deadAnimationBuffer++;
                _animationController.Update(gameTime);
                Animation?.Update(gameTime);
                CurrentState.Update(gameTime);
            }
            else{
                if (health == 0){
                    IsDead = true;
                    //play pop animation
                }
                else
                {
                    health -= 1;
                    deadAnimationBuffer = 0;
                    ChangeState(new AquamentusLeftState(this));
                }
            }
        }
    }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    /// <summary>
    ///     Uses the velocity to update aquamentus's position.
    /// </summary>
    private void UpdatePosition()
    {
        Position += Velocity;
    }

    public void MoveHorizontal(bool IsFacingLeft)
    {
        if (LeftRightBuffer < 100)
        {
            LeftRightBuffer += 1;
            Velocity = new Vector2(1, 0);
        }
        else
        {
            LeftRightBuffer += 1;
            Velocity = new Vector2(-1, 0);
        }

        if (LeftRightBuffer > 200) LeftRightBuffer = 0;
    }

    private void UpdateDirection()
    {
        var playerPostion = _player.Position;
        if (playerPostion.X < Position.X)
            IsFacingLeft = false;
        else
            IsFacingLeft = true;
    }

    public void FlipDirection()
    {
        Velocity = new Vector2(-Velocity.X, Velocity.Y);
        IsFacingLeft = !IsFacingLeft;
    }

    private bool CheckFireballBuffer()
    {
        if (CurrentState is AquamentusLeftState || CurrentState is AquamentusRightState)
        {
            if (FireballsBuffer < 250)
            {
                FireballsBuffer += 1;
            }
            else
            {
                FireballsBuffer = 0;
                return true;
            }
        }

        return false;
    }

    private void ThrowFireball()
    {
        
        fireball1 = new AquamentusProjectile(Position.X, Position.Y, IsFacingLeft, MathHelper.ToRadians(25), this);
        _projectiles.Add(fireball1);

        fireball2 = new AquamentusProjectile(Position.X, Position.Y, IsFacingLeft, 0, this);
        _projectiles.Add(fireball2);

        fireball3 = new AquamentusProjectile(Position.X, Position.Y, IsFacingLeft, MathHelper.ToRadians(-25), this);
        _projectiles.Add(fireball3);
    }
}