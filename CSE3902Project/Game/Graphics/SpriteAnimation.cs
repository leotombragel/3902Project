using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

public class SpriteAnimation : ISprite
{
    private readonly int _bufferWidth; //some sprite sheets have a buffer between frames
    private readonly int _frameCount;
    private readonly float _frameDuration;
    private readonly int _frameHeight;
    private readonly bool _loops = true;
    private readonly int[] _frameOrder;

    private readonly int _frameWidth;
    private readonly bool _hasBufferWidth; //initailzed to false

    // Sprite animation properties
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
        Sprite = sprite;
        _frameCount = frameCount;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _frameDuration = frameDuration;
        _startingX = 0;
        _startingY = 0;

        _hasBufferWidth = false;

        InitializeSourceRectangle();
    }

    /// <summary>
    ///     Sets the required parameters for a sprite animation.
    /// </summary>
    /// <param name="sprite">The sprite to be animated.</param>
    /// <param name="frameWidth">The width of the rectangle representing a single frame.</param>
    /// <param name="frameHeight">The height of the rectangle representing a single frame.</param>
    /// <param name="frameCount">The number of frames.</param>
    /// <param name="frameDuration">The duration of a single frame, in seconds.</param>
    /// <param name="startingX">The x-coordinate of the starting position of the first frame.</param>
    /// <param name="startingY">The y-coordinate of the starting position of the first frame.</param>
    public SpriteAnimation(Sprite sprite, int frameWidth, int frameHeight, int frameCount, float frameDuration,
        int startingX, int startingY, bool loops = true)
    {
        Sprite = sprite;
        _frameCount = frameCount;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _frameDuration = frameDuration;
        _loops = loops;
        _startingX = startingX;
        _startingY = startingY;

        _hasBufferWidth = false;

        InitializeSourceRectangle();
    }

    /// <summary>
    ///     Sets the required parameters for a sprite animation. this includes bufferWidth
    /// </summary>
    /// <param name="sprite">The sprite to be animated.</param>
    /// <param name="frameWidth">The width of the rectangle representing a single frame.</param>
    /// <param name="frameHeight">The height of the rectangle representing a single frame.</param>
    /// <param name="frameCount">The number of frames.</param>
    /// <param name="frameDuration">The duration of a single frame, in seconds.</param>
    /// <param name="startingX">The x-coordinate of the starting position of the first frame.</param>
    /// <param name="startingY">The y-coordinate of the starting position of the first frame.</param>
    /// <param name="bufferWidth">The width of the buffer between frames.</param>
    public SpriteAnimation(Sprite sprite, int frameWidth, int frameHeight, int frameCount, float frameDuration,
        int startingX, int startingY, int bufferWidth)
    {
        Sprite = sprite;
        _frameCount = frameCount;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _frameDuration = frameDuration;
        _startingX = startingX;
        _startingY = startingY;
        _bufferWidth = bufferWidth;

        _hasBufferWidth = true;

        InitializeSourceRectangle();
    }

    public SpriteAnimation(Sprite sprite, int frameWidth, int frameHeight, int[] frameOrder,
        float frameDuration, int startingX, int startingY, int bufferWidth = 0, bool loops = true)
    {
        ArgumentNullException.ThrowIfNull(frameOrder);
        if (frameOrder.Length == 0)
        {
            throw new ArgumentException("Animation frame order cannot be empty.", nameof(frameOrder));
        }

        Sprite = sprite;
        _frameWidth = frameWidth;
        _frameHeight = frameHeight;
        _frameCount = frameOrder.Length;
        _frameDuration = frameDuration;
        _frameOrder = (int[])frameOrder.Clone();
        _startingX = startingX;
        _startingY = startingY;
        _bufferWidth = bufferWidth;
        _hasBufferWidth = bufferWidth > 0;
        _loops = loops;

        InitializeSourceRectangle();
    }

    // Sprite animation properties
    public Sprite Sprite { get; set; }

    public int LoopCount { get; private set; }

    public bool IsPlaying { get; } = true;

    public void Update(GameTime gameTime)
    {
        if (!IsPlaying || (!_loops && LoopCount > 0)) return;

        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timer >= _frameDuration)
        {
            _timer -= _frameDuration;

            var isLastFrame = _currentFrame == _frameCount - 1;
            if (isLastFrame)
            {
                LoopCount++;
                if (!_loops) return;
                _currentFrame = 0;
            }
            else
            {
                _currentFrame++;
            }

            UpdateSourceRectangle();
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        Sprite.Draw(spriteBatch, position);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, bool isFacingLeft)
    {
        Sprite.Draw(spriteBatch, position, isFacingLeft);
    }

    private void InitializeSourceRectangle()
    {
        _currentFrame = 0;
        _timer = 0.0f;
        LoopCount = 0;
        UpdateSourceRectangle();
    }

    /// <summary>
    ///     Updates the source rectangle of the sprite to the current frame of the animation.
    /// </summary>
    private void UpdateSourceRectangle()
    {
        var frameStride = _frameWidth + (_hasBufferWidth ? _bufferWidth : 0);
        var frameIndex = _frameOrder is null ? _currentFrame : _frameOrder[_currentFrame];
        var x = _startingX + frameIndex * frameStride;
        var y = _startingY;
        Sprite.SourceRectangle = new Rectangle(x, y, _frameWidth, _frameHeight);
    }

    /// <summary>
    ///     Resets the animation to the first frame and sets the timer to zero.
    /// </summary>
    public void Reset()
    {
        _currentFrame = 0;
        _timer = 0.0f;
        LoopCount = 0;
        UpdateSourceRectangle();
    }
}