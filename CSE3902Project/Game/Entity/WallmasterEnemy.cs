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
///     Wallmaster enemy class, implements StatefulEntityBase, IEnemy
/// </summary>
public class WallmasterEnemy : StatefulEntityBase, IEnemy
{
    //movement
    public Vector2 Velocity { get; private set; }
    public Vector2 Position { get; private set; }
    private readonly int StartingPosY = 70;
    private const int StartingPosX = 600;

    //animation
    private readonly AnimationController _animationController;
    public Player _player;
    private IHurtbox _bodyHurtbox;

    private int TrackingBuffer = 0;

    public WallmasterEnemy(Player player)
    {
        Sprite = SpriteFactory.Instance.CreateWallmasterSprite();
        _animationController = new AnimationController(this, new WallmasterAnimationFactory());
        CurrentState = new WallmasterWaitingState(this);
        Velocity = new Vector2(0, 1);
        Position = new Vector2(StartingPosX, StartingPosY);

        _player = player;
        _bodyHurtbox = new BodyHurtbox(this, new Point(0, 0), new Point(34, 40));
    }

    public WallmasterEnemy(Sprite sprite, Player player)
    {
        Sprite = sprite;
        _animationController = new AnimationController(this, new WallmasterAnimationFactory());
        CurrentState = new WallmasterWaitingState(this);
        Velocity = new Vector2(0, 1);
        Position = new Vector2(StartingPosX, StartingPosY);

        _player = player;
        _bodyHurtbox = new BodyHurtbox(this, new Point(0, 0), new Point(34, 40));
    }

    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }

    //state
    public bool IsFacingLeft { get; private set; }
    public bool IsDead { get; set; } // for list culling later
    private int deadAnimationBuffer = 0;

    public void TakeDamage(DamageInfo damageInfo)
    {
        ChangeState(new WallmasterDeadState(this));
    }

    public DamageTeam Team => DamageTeam.Enemy;

    //sprite sheet from https://www.spriters-resource.com/nes/legendofzelda/asset/31806/
    public void Draw(SpriteBatch spriteBatch)
    {
        _animationController.Draw(spriteBatch, Position, IsFacingLeft);
    }

    public virtual void Update(GameTime gameTime)
    {
        if (CurrentState is not WallmasterDeadState){
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

    public bool CheckTrackingBuffer()
    {
        if (CurrentState is WallmasterTrackingState)
        {
            if (TrackingBuffer < 150)
            {
                TrackingBuffer += 1;
            }
            else
            {
                TrackingBuffer = 0;
                return true;
            }
        }

        return false;
    }

    private void UpdatePosition()
    {
        if (CheckTrackingBuffer()) ChangeState(new WallmasterMovingDownState(this));

        if (CurrentState is WallmasterWaitingState){
                if (_player.Position.X > Position.X -10 && _player.Position.X < Position.X + 10){
                    ChangeState(new WallmasterTrackingState(this));
                }
        }
        else if (CurrentState is WallmasterTrackingState){
                Position = new Vector2(_player.Position.X , Position.Y);
        }
        else if(CurrentState is WallmasterMovingDownState){
                Velocity = new Vector2(0, 3);
                Position += Velocity;

                if (Position.Y >= 400){ //need real collision detection
                    ChangeState(new WallmasterMovingUpState(this));
                }
        }
        else if(CurrentState is WallmasterMovingUpState){
            Velocity = new Vector2(0, -6);
            Position += Velocity;

            if (Position.Y <= StartingPosY){ 
                ChangeState(new WallmasterRecoverState(this));
            }
        }
        else if(CurrentState is WallmasterRecoverState){
            if(_player.Position.X > Position.X){
                Velocity = new Vector2(4, 0);
            } 
            else if(_player.Position.X < Position.X){
                Velocity = new Vector2(-4, 0);
            }
            Position += Velocity;

            if (_player.Position.X > Position.X -10 && _player.Position.X < Position.X + 10){
                    ChangeState(new WallmasterTrackingState(this));
            }
        }
    }
}


