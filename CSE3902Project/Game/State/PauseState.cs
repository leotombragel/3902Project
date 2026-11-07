using CSE3902Project.Game.Command;
using CSE3902Project.Game.Input;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CSE3902Project.Game.State;

public class PauseState : IGameState
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly KeyboardController _keyboardController;
    private readonly Texture2D _blackTexture;
    private readonly StackPanel _menu;

    public PauseState(GraphicsDevice graphics, StateManager stateManager, Game1 game)
    {
        _graphicsDevice = graphics;
        _keyboardController = new KeyboardController();

        var resumeCommand = new ResumeCommand(stateManager);
        var exitCommand = new ExitCommand(game);
        
        _keyboardController.RegisterPressCommand(Keys.Escape, resumeCommand);
        _keyboardController.RegisterPressCommand(Keys.Q, exitCommand);
        
        _menu = new StackPanel
        {
            Spacing = 10
        };
        _menu.Anchor(Anchor.Center);

        var label = new Label
        {
            Text = "Paused"
        };
        label.Anchor(Anchor.Center);

        var resumeButton = new Button
        {
            Text = "Resume",
            Width = 200,
            Height = 50
        };
        
        var exitButton = new Button
        {
            Text = "Quit",
            Width = 200,
            Height = 50
        };
        
        resumeButton.Click += (_, _) => resumeCommand.Execute();
        exitButton.Click += (_, _) => exitCommand.Execute();
        
        _menu.AddChild(label);
        _menu.AddChild(resumeButton);
        _menu.AddChild(exitButton);

        _blackTexture = new Texture2D(_graphicsDevice, 1, 1);
        Color[] colorData = [Color.Black];
        _blackTexture.SetData(colorData);
    }

    public void Enter()
    {
        _keyboardController.SyncState();

        _menu.AddToRoot();
    }

    public void Exit()
    {
        _menu.RemoveFromRoot();
    }

    public void Update(GameTime gameTime)
    {
        _keyboardController.Update();
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_blackTexture,
            new Rectangle(0, 0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height), Color.White * 0.5f);
        spriteBatch.End();
    }
}