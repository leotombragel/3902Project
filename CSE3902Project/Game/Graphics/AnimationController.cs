using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// <summary>
///     This class manages an entity's animations based on its current state. If the entity's state changes, AnimationController
///     will update the current animation to match. It also handles updating and drawing the current animation sprite.
/// </summary>
public class AnimationController
{
    /// <summary>
    /// The animation factory for the entity.
    /// </summary>
    private readonly IAnimationFactory _animationFactory;

    private readonly IAnimatable _entity;
    private string _currentAnimationName;

    public AnimationController(IAnimatable entity, IAnimationFactory animationFactory)
    {
        _entity = entity;
        _animationFactory = animationFactory;
        CurrentSprite = null;
        _currentAnimationName = null;
    }

    public ISprite CurrentSprite { get; set; }

    public void Update(GameTime gameTime)
    {
        if (_entity.CurrentState.AnimationName != _currentAnimationName)
        {
            _currentAnimationName = _entity.CurrentState.AnimationName;
            CurrentSprite = _animationFactory.Create(_currentAnimationName);
            _entity.Animation = CurrentSprite as SpriteAnimation;
        }

        CurrentSprite?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, bool isFacingLeft)
    {
        CurrentSprite?.Draw(spriteBatch, position, isFacingLeft);
    }
}