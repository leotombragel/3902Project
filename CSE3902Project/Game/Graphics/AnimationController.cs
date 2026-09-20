using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

public class AnimationController
{
    private readonly IAnimatable _entity;
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