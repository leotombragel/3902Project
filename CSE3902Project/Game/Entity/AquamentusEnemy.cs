using System;
using System.Collections.Generic;
using CSE3902Project.Game.Entity.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSE3902Project.Game.Graphics;

namespace CSE3902Project.Game.Entity;

/// <summary>
/// aquamentus enemy class, implements IMortal
/// </summary>
public class AquamentusEnemy : IMortal
{
    //animation
    private readonly AnimationController _animationController;
    public Sprite Sprite { get; set; }
    public SpriteAnimation Animation { get; set; }
    public int FrameBuffer = 0;//Stalfo has no animation, just a frame flipped over the y axis

    //state
    public bool IsFacingLeft { get; private set; }
    public IState CurrentState { get; private set;}
    public bool IsDead { get; set; }

    //movement
    public Vector2 Position { get; private set; }
    public Vector2 Velocity { get; private set; }
    private const int StartingPosX = 200; //figure out how to set these through constructor late
    private const int StartingPosY = 100; 
    public int LeftRightBuffer = 0;//use this until we get collision to switch direction like a goomba

    // references
    private Player _player;
    private List<IProjectile> _projectiles;

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
    }

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
            {
                IsFacingLeft = false;
            }
            else
            {
                IsFacingLeft = true;
            }
            FrameBuffer = 0;
        }
    }

    /// <summary>
    /// Uses the velocity to update the keese's position.
    /// </summary>
    private void UpdatePosition()
    {
        Position += Velocity;
    }

    private void UpdateDirection()
    {
        var playerPostion = _player.Position;
        if (playerPostion.X < Position.X)
        {
            IsFacingLeft = true;
        }
        else
        {
            IsFacingLeft = false;
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        _animationController.Update(gameTime);
        Animation?.Update(gameTime);
        CurrentState.Update(gameTime);

        UpdateDirection();
    }

    public void MoveVertical()
    {
    //wait to implement when we get collision
    }

    public bool MoveHorizontal(bool IsFacingLeft)
    {
    //wait to implement further when we get proper collision
    if (LeftRightBuffer < 400)
    {
        LeftRightBuffer += 1;
    }
    else
    {
           LeftRightBuffer = 0;
           IsFacingLeft = !IsFacingLeft;
           Velocity = new Vector2(Velocity.X * -1, Velocity.Y);

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