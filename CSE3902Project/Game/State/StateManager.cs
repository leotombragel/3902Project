using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.State;

public class StateManager
{
    private IGameState _currentState;
    
    public StateManager()
    {
        _currentState = null!;
    }
    
    public StateManager(IGameState initialState)
    {
        _currentState = initialState;
    }
    
    public void ChangeState(IGameState newState)
    {
        if (_currentState == newState && _currentState != null) return;
        
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
    
    public void Update(GameTime gameTime)
    {
        _currentState.Update(gameTime);
    }
    
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _currentState.Draw(gameTime, spriteBatch);
    }
}