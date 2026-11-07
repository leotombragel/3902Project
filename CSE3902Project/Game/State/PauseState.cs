using CSE3902Project.Game.Command;
using CSE3902Project.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CSE3902Project.Game.State;

public class PauseState : IGameState
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly KeyboardController _keyboardController;

    public PauseState(GraphicsDevice graphics, StateManager stateManager, Game1 game)
    {
        _graphicsDevice = graphics;
        _keyboardController = new KeyboardController();

        _keyboardController.RegisterPressCommand(Keys.Escape, new ResumeCommand(stateManager));
        _keyboardController.RegisterPressCommand(Keys.Q, new ExitCommand(game));
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update(GameTime gameTime)
    {
        _keyboardController.Update();
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        Color[] colorData = { Color.Black };
        var blackTexture = new Texture2D(_graphicsDevice, 1, 1);
        blackTexture.SetData(colorData);
        
        spriteBatch.Begin();
        spriteBatch.Draw(blackTexture,
            new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White * 0.5f);
        spriteBatch.End();
    }
}