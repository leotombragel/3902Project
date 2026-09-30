using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

/// <summary>
///     This class manages an entity's animations based on its current state. If the entity's state changes,
///     AnimationController
///     will update the current animation to match. It also handles updating and drawing the current animation sprite.
/// </summary>
public class AnimationController
{
    /// <summary>
    ///     The animation source (factory) for the entity
    /// </summary>
    private readonly IAnimationSource _animationSource;

    private readonly IAnimatable _entity;
    private string _currentAnimationName;

    public AnimationController(IAnimatable entity, IAnimationSource source)
    {
        _entity = entity;
        _animationSource = source;
        CurrentSprite = null;
        _currentAnimationName = null;
    }

    public ISprite CurrentSprite { get; set; }

    public void Update(GameTime gameTime)
    {
        if (_entity.CurrentState.AnimationName != _currentAnimationName)
        {
            _currentAnimationName = _entity.CurrentState.AnimationName;

            // Get the correct sprite from the sprite factory
            CurrentSprite = _animationSource.Create(_currentAnimationName);

            // Update the entity's animation reference so external classes can access it
            _entity.Animation = CurrentSprite as SpriteAnimation;
        }

        CurrentSprite?.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, bool isFacingLeft)
    {
        CurrentSprite?.Draw(spriteBatch, position, isFacingLeft);
    }
}