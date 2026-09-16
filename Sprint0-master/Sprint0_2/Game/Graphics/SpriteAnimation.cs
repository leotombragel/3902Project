using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0_2.Game.Graphics;

public class SpriteAnimation : ISprite
{
    // Sprite animation properties
    private readonly Sprite _sprite;
    private readonly int _frameCount;
    private readonly int _frameWidth;
    private readonly int _frameHeight;
    private readonly float _frameDuration;
    
    // Current animation data
    private int _currentFrame;
    private float _timer;
    
    public bool IsPlaying { get; private set; } = true;
    
    public SpriteAnimation(Sprite sprite, int frameWidth, int frameHeight, int frameCount, float frameDuration)
    {
        _sprite = sprite;
        _frameCount = frameCount;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _frameDuration = frameDuration;
        
        // Initialize the source rectangle to the first frame
        _currentFrame = 0;
        _timer = 0.0f;
        UpdateSourceRectangle();
    }
    
    public void Update(GameTime gameTime)
    {
        if (!IsPlaying) return;
        
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (_timer >= _frameDuration)
        {
            // Switch frames
            _timer -= _frameDuration;
            _currentFrame = (_currentFrame + 1) % _frameCount;
            UpdateSourceRectangle();
        }
    }
    
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        _sprite.Draw(spriteBatch, position);
    }
    
    /// <summary>
    /// Updates the source rectangle of the sprite to the current frame of the animation.
    /// </summary>
    private void UpdateSourceRectangle()
    {
        var x = _currentFrame * _frameWidth;
        var y = 0;
        _sprite.SourceRectangle = new Rectangle(x, y, _frameWidth, _frameHeight);
    }

    /// <summary>
    /// Resets the animation to the first frame and sets the timer to zero.
    /// </summary>
    public void Reset()
    {
        _currentFrame = 0;
        _timer = 0.0f;
        UpdateSourceRectangle();
    }
}