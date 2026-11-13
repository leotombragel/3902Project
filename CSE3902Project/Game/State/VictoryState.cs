using System;
using CSE3902Project.Game.Graphics;
using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RectangleRuntime = Gum.GueDeriving.RectangleRuntime;

namespace CSE3902Project.Game.State;

public class VictoryState : IGameState
{
    private readonly GameplayState _gameplayState;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly StateManager _stateManager;
    
    private StackPanel _menu;
    private RectangleRuntime _overlay;

    public VictoryState(GameplayState gameplayState, GraphicsDevice graphicsDevice, StateManager stateManager)
    {
        _gameplayState = gameplayState;
        _graphicsDevice = graphicsDevice;
        _stateManager = stateManager;
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
            Text = "Victory",
        };
        label.Anchor(Anchor.Center);

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
            FillColor = Color.Chartreuse * 0.9f,
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