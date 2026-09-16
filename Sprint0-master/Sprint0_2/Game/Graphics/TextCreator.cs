using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0_2.Game.Graphics;

public class TextCreator
{
    private static SpriteFont _font;
    
    public static void Initialize(ContentManager content)
    {
        _font = content.Load<SpriteFont>("fonts/game");
    }
    
    public static void CreateText(string text, Vector2 position, SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_font, text, position, Color.Black);
    }

    public static void CreateSprint0Text(GameWindow window, SpriteBatch spriteBatch)
    {
        CreateText(
            """
            Use A/D for walking, space to jump, and click for rocking chair
            Credits
            Program made by: Uthman Wood
            Font is Noto Sans from Google Fonts
            Sprites retrieved from Super Mario Wiki
            Gifs were converted to PNG sprite sheets using ImageMagick
            
            https://www.mariowiki.com/File:Cranky_Kong_DKC_walking.gif
            https://www.mariowiki.com/Gallery:Cranky_Kong#/media/File:CrankyKongCountry.gif
            https://www.mariowiki.com/File:Cranky_Kong_DKC_sprite.png
            """,
            new Vector2(window.ClientBounds.Width * 0.05f,
                window.ClientBounds.Height * 0.4f),
            spriteBatch);
    }
}