using CSE3902Project.Game.Collision;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Entity.State;
using CSE3902Project.Game.Entity.State.Player_States;
using CSE3902Project.Game.Graphics;
using CSE3902Project.Game.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CSE3902Project.Game.Entity;

/// <summary>
///     stalfo enemy class, implements StatefulEntityBase, IEnemy
/// </summary>
public class StalfoEnemy : StatefulEntityBase, IEnemy
{
    private const float VerticalMoveSpeed = 4.0f;
    private const float MaxSpeed = 3.0f;

    private const int StartingPosX = 600; //figure out how to set these through constructor late

    //animation
    private readonly AnimationController _animationController;
    private readonly int StartingPosY = 100;
    public int FrameBuffer; //Stalfo has no animation, just a frame flipped over the y axis
    public readonly IHurtbox _bodyHurtbox;

    public Rect Hitbox;
    private StalfoStageCollision _stageCollision;

    public int LeftRightBuffer; //use this until we get collision to switch direction like a goomba


    public StalfoEnemy()
    {
        Sprite = SpriteFactory.Instance.CreateStalfoSprite();
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = new Vector2(-1, 0);
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new StalfoAnimationFactory());
        CurrentState = new StalfoLeftState(this);
        _bodyHurtbox = new BodyHurtbox(this, new Point(-7, 0), new Point(46, 32));
    }

    public StalfoEnemy(Vector2 spawnPosition)
        : this()
    {
        Position = spawnPosition;

    }

    public StalfoEnemy(Sprite sprite)
    {
        Sprite = sprite;
        Velocity = new Vector2(-1, 0);
        Position = new Vector2(StartingPosX, StartingPosY);
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new StalfoAnimationFactory());
        CurrentState = new StalfoLeftState(this);
        _bodyHurtbox = new BodyHurtbox(this, new Point(-7, 0), new Point(46, 32));
    }

    public StalfoEnemy(int GroundHeight)
    {
        Sprite = SpriteFactory.Instance.CreateStalfoSprite();
        var spriteHeight = Sprite.SourceRectangle?.Height ?? 0;
        StartingPosY =
            GroundHeight - 32 -
            spriteHeight; // Assuming the stalfo sprite is 32 pixels tall and we want it to be above the ground tile
        Position = new Vector2(StartingPosX, StartingPosY);
        Velocity = new Vector2(-1, 0);
        IsFacingLeft = true;
        _animationController = new AnimationController(this, new StalfoAnimationFactory());
        CurrentState = new StalfoLeftState(this);
        _bodyHurtbox = new BodyHurtbox(this, new Point(-7, 0), new Point(46, 32));
    }

    public Sprite Sprite { get; set; }
    public Vector2 Velocity { get; set; }
    public SpriteAnimation Animation { get; set; }

    //state
    public bool IsFacingLeft { get; set; }
    private int deadAnimationBuffer = 0;
    public bool IsDead { get; set; }

    public void TakeDamage(DamageInfo damageInfo)
    {
        ChangeState(new StalfoDeadState(this));
        //Console.WriteLine($"Stalfo took damage. Source: {damageInfo.Source}");
    }

    //movement
    public Vector2 Position { get; set; }

    public DamageTeam Team => DamageTeam.Enemy;

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
                IsFacingLeft = false;
            else
                IsFacingLeft = true;
            FrameBuffer = 0;
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        if (CurrentState is not StalfoDeadState){
            _animationController.Update(gameTime);
            Animation?.Update(gameTime);
            CurrentState.Update(gameTime);

            IsFacingLeft = MoveHorizontal(IsFacingLeft);
            UpdatePosition();
        }
        else{
            if (deadAnimationBuffer < 8)
            {
                deadAnimationBuffer++;
                _animationController.Update(gameTime);
                Animation?.Update(gameTime);
                CurrentState.Update(gameTime);
            }
            else{
                IsDead = true;
                //play pop animation
            }
        }
    }

    public IEnumerable<IHurtbox> Hurtboxes
    {
        get { yield return _bodyHurtbox; }
    }

    public void SetStageCollision(StalfoStageCollision stageCollision)
    {
        _stageCollision = stageCollision;
    }

    /// <summary>
    ///     Uses the velocity to update the stalfo's position.
    /// </summary>
    private void UpdatePosition()
    {
        if (_stageCollision == null)
        {
            Position += Velocity;
            return;
        }

        _stageCollision.MoveStalfo(this);
    }

    public void MoveVertical()
    {
        //wait to implement when we get collision
    }

    public bool MoveHorizontal(bool IsFacingLeft)
    {
        if (LeftRightBuffer < 400)
        {
            LeftRightBuffer += 1;
        }
        else
        {
            LeftRightBuffer = 0;
            flipDirection();
        }

        return IsFacingLeft;
    }


    public void flipDirection()
    {
        Velocity = new Vector2(-Velocity.X, Velocity.Y);
        IsFacingLeft = !IsFacingLeft;
    }
}