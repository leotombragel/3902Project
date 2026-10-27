using CSE3902Project.Game.Collision;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Entity.State;
using CSE3902Project.Game.Graphics;
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
    //movement
    public Vector2 Position { get; set; }
    private const int StartingPosX = 600; //overridden by csv file
    private readonly int StartingPosY = 100; //overridden by csv file
    public Vector2 Velocity { get; set; }
    public IHurtbox _bodyHurtbox { get; }
    private StalfoStageCollision _stageCollision;

    //animation
    private readonly AnimationController _animationController;
    public int FrameBuffer; //Stalfo has no animation, just a frame flipped over the y axis
    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    //state
    public bool IsFacingLeft { get; set; }
    private int deadAnimationBuffer = 0;
    public bool IsDead { get; set; }

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

    public void TakeDamage(DamageInfo damageInfo)
    {
        ChangeState(new StalfoDeadState(this));
    }

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
            IsFacingLeft = !IsFacingLeft;
            FrameBuffer = 0;
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        if (CurrentState is not StalfoDeadState){
            _animationController.Update(gameTime);
            Animation?.Update(gameTime);
            CurrentState.Update(gameTime);

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

        _stageCollision.MoveSideways(this);
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
            FlipDirection();
        }

        return IsFacingLeft;
    }


    public void FlipDirection()
    {
        Velocity = new Vector2(-Velocity.X, Velocity.Y);
        IsFacingLeft = !IsFacingLeft;
    }
}