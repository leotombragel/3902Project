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
    private readonly StackPanel _menu;
    private readonly RectangleRuntime _overlay;

    public VictoryState(GameplayState gameplayState, GraphicsDevice graphicsDevice)
    {
        _gameplayState = gameplayState;
        _graphicsDevice = graphicsDevice;
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
    }
    
    public void Enter()
    {
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