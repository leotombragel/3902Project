using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.State;

public class StateManager
{
    private readonly Stack<IGameState> _activeStates;

    public StateManager()
    {
        _activeStates = new Stack<IGameState>();
    }

    public StateManager(IGameState initialState)
    {
        _activeStates = new Stack<IGameState>();
        _activeStates.Push(initialState);
        initialState.Enter();
    }

    public void Push(IGameState newState)
    {
        _activeStates.TryPeek(out var currentState);

        if (currentState == newState && currentState != null) return;
        
        _activeStates.Push(newState);
        newState?.Enter();
    }

    public IGameState Pop()
    {
        _activeStates.TryPeek(out var currentState);
        currentState?.Exit();
        return _activeStates.Pop();
    }

    public void Update(GameTime gameTime)
    {
        // Only the top state is updated, the others may be visible but frozen.
        _activeStates.TryPeek(out var currentState);
        currentState?.Update(gameTime);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        // Enumerate from the bottom to the top. This lets the bottom state be drawn first, and the top state be drawn last (on top).
        var statesToDraw = _activeStates.Reverse().ToList();
        foreach (var state in statesToDraw)
        {
            state.Draw(gameTime, spriteBatch);
        }
    }
}