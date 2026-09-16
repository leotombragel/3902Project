using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0_2.Game.Graphics;

public class Sprite : ISprite
{
    /// <summary>
    /// The color to tint the sprite with. Default is white (no tint).
    /// </summary>
    public Color Color { get; set; } = Color.White;
    
    /// <summary>
    /// The rotation of the sprite in radians. Default is 0.0f.
    /// </summary>
    public float Rotation { get; set; } = 0.0f;
    
    /// <summary>
    /// The scale of the sprite. Default is 1.0f (no scaling).
    /// </summary>
    public Vector2 Scale { get; set; } = Vector2.One;
    
    /// <summary>
    /// The origin point of the sprite. Default is (0, 0) (top-left corner).
    /// </summary>
    public Vector2 Origin { get; set; } = Vector2.Zero;
    
    /// <summary>
    /// The effects to apply to the sprite. Default is None.
    /// </summary>
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;
    
    public Texture2D Texture { get; set; }
    
    /// <summary>
    /// The source rectangle of the sprite. Default is null (use the entire texture).
    /// </summary>
    public Rectangle? SourceRectangle { get; set; } = null;
    

    public Sprite()
    {
    }

    public Sprite(Texture2D texture)
    {
        Texture = texture;
    }
    
    public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(Texture, position, SourceRectangle, Color, Rotation, Origin, Scale, Effects, 0.0f);
    }
    
    public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, bool isFlippedHorizontally)
    {
        Effects = isFlippedHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        Draw(spriteBatch, position);
    }

    public virtual void Update(GameTime gameTime)
    {
    }
}