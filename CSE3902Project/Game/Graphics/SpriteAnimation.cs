using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

public class SpriteAnimation : ISprite
{
    private readonly int _frameCount;
    private readonly float _frameDuration;
    private readonly int _frameHeight;

    private readonly int _frameWidth;

    // Sprite animation properties
    private readonly Sprite _sprite;
    private readonly int _startingX;
    private readonly int _startingY;

    // Current animation data
    private int _currentFrame;
    private float _timer;

    /// <summary>
    ///     This constructor assumes the animation starts in the top left corner. It should not be used for sprite sheets with
    ///     multiple rows.
    /// </summary>
    /// <param name="sprite">The sprite to be animated.</param>
    /// <param name="frameWidth">The width of the rectangle representing a single frame.</param>
    /// <param name="frameHeight">The height of the rectangle representing a single frame.</param>
    /// <param name="frameCount">The number of frames.</param>
    /// <param name="frameDuration">The duration of a single frame, in seconds.</param>
    public SpriteAnimation(Sprite sprite, int frameWidth, int frameHeight, int frameCount, float frameDuration)
    {
        _sprite = sprite;
        _frameCount = frameCount;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _frameDuration = frameDuration;
        _startingX = 0;
        _startingY = 0;

        InitializeSourceRectangle();
    }

    /// <summary>
    /// Sets the required parameters for a sprite animation.
    /// </summary>
    /// <param name="sprite">The sprite to be animated.</param>
    /// <param name="frameWidth">The width of the rectangle representing a single frame.</param>
    /// <param name="frameHeight">The height of the rectangle representing a single frame.</param>
    /// <param name="frameCount">The number of frames.</param>
    /// <param name="frameDuration">The duration of a single frame, in seconds.</param>
    /// <param name="startingX">The x-coordinate of the starting position of the first frame.</param>
    /// <param name="startingY">The y-coordinate of the starting position of the first frame.</param>
    public SpriteAnimation(Sprite sprite, int frameWidth, int frameHeight, int frameCount, float frameDuration,
        int startingX, int startingY)
    {
        _sprite = sprite;
        _frameCount = frameCount;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _frameDuration = frameDuration;
        _startingX = startingX;
        _startingY = startingY;

        InitializeSourceRectangle();
    }

    public bool IsPlaying { get; } = true;

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

    public void Draw(SpriteBatch spriteBatch, Vector2 position, bool isFacingLeft)
    {
        _sprite.Draw(spriteBatch, position, isFacingLeft);
    }

    private void InitializeSourceRectangle()
    {
        _currentFrame = 0;
        _timer = 0.0f;
        UpdateSourceRectangle();
    }

    /// <summary>
    ///     Updates the source rectangle of the sprite to the current frame of the animation.
    /// </summary>
    private void UpdateSourceRectangle()
    {
        var x = _startingX + _currentFrame * _frameWidth;
        var y = _startingY;
        _sprite.SourceRectangle = new Rectangle(x, y, _frameWidth, _frameHeight);
    }

    /// <summary>
    ///     Resets the animation to the first frame and sets the timer to zero.
    /// </summary>
    public void Reset()
    {
        _currentFrame = 0;
        _timer = 0.0f;
        UpdateSourceRectangle();
    }
}