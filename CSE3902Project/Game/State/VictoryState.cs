using CSE3902Project.Game.Graphics;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RenderingLibrary.Graphics;

namespace CSE3902Project.Game.State;

public class VictoryState : IGameState
{
    private readonly GameplayState _gameplayState;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly StackPanel _menu;
    
    public VictoryState(GameplayState gameplayState, GraphicsDevice graphicsDevice)
    {
        _gameplayState = gameplayState;
        _graphicsDevice = graphicsDevice;
        _menu = new StackPanel
        {
            Orientation = Orientation.Vertical,
        };
    }
    
    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update(GameTime gameTime)
    {
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        TextCreator.CreateHyliaSerifText("Victory", Vector2.Zero, spriteBatch, Color.White);
    }
}