using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Graphics;

public interface ISprite
{
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch, Vector2 position);
}