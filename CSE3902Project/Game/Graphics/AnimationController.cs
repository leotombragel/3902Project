using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// <summary>
/// This class manages an entity's animations based on its current state. If the entity's state changes, AnimationController
/// will update the current animation to match. It also handles updating and drawing the current animation sprite.
/// </summary>
public class AnimationController
{
    private readonly IAnimatable _entity;
    /// <summary>
    /// The animation source (factory) for the entity
    /// </summary>
    private readonly IAnimationSource _animationSource;

    private ISprite _currentSprite;
    private string _currentAnimationName;

    public AnimationController(IAnimatable entity, IAnimationSource source)
    {
        _entity = entity;
        _animationSource = source;
        _currentSprite = null;
        _currentAnimationName = null;
    }

    public void Update(GameTime gameTime)
    {
        if (_entity.CurrentState.AnimationName != _currentAnimationName)
        {
            _currentAnimationName = _entity.CurrentState.AnimationName;
            _currentSprite = _animationSource.Create(_currentAnimationName);
        }
        
        _currentSprite?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, bool isFacingLeft)
    {
        _currentSprite?.Draw(spriteBatch, position, isFacingLeft);
    }
}