using System;
using CSE3902Project.Game.Graphics;
using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RectangleRuntime = Gum.GueDeriving.RectangleRuntime;

namespace CSE3902Project.Game.State;

public class GameOverState : IGameState
{
    private readonly GameplayState _gameplayState;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly StateManager _stateManager;
    
    private readonly Color _bgColor;
    private readonly string _titleText;
    
    private StackPanel _menu;
    private RectangleRuntime _overlay;

    public GameOverState(GameplayState gameplayState, GraphicsDevice graphicsDevice, StateManager stateManager, Color bgColor, string titleText)
    {
        _gameplayState = gameplayState;
        _graphicsDevice = graphicsDevice;
        _stateManager = stateManager;
        _bgColor = bgColor;
        _titleText = titleText;
        SetupUi();
    }

    private void HandlePlayAgain()
    {
        _gameplayState.ResetGame();
        _stateManager.Pop();
    }

    private void SetupUi()
    {
        _menu = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Spacing = 10f
        };
        _menu.Anchor(Anchor.Center);

        var label = new Label
        {
            Text = _titleText,
        };
        label.Anchor(Anchor.Top);

        var button = new Button
        {
            Text = "Play Again",
            Width = 200,
            Height = 50
        };
        button.Anchor(Anchor.Center);
        button.Click += (_, _) => HandlePlayAgain();

        _overlay = new RectangleRuntime
        {
            IsFilled = true,
            FillColor = _bgColor * 0.9f,
            WidthUnits = DimensionUnitType.PercentageOfParent,
            HeightUnits = DimensionUnitType.PercentageOfParent,
            Width = 100f,
            Height = 100f
        };
        
        _menu.AddChild(label);
        _menu.AddChild(button);
    }
    
    public void Enter()
    {
        MusicFactory.StopMusic();
        _overlay.AddToRoot();
        _menu.AddToRoot();
    }

    public void Exit()
    {
        _overlay.RemoveFromRoot();
        _menu.RemoveFromRoot();
    }

    public void Update(GameTime gameTime)
    {
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
    }
}