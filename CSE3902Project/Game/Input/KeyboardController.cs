using System.Collections.Generic;
using CSE3902Project.Game.Command;
using Microsoft.Xna.Framework.Input;

namespace CSE3902Project.Game.Input;

public class KeyboardController : IController
{
    private readonly Dictionary<Keys, ICommand> _keyBindings = new();
    private readonly Dictionary<Keys, ICommand> _pressBindings = new();
    private KeyboardState _previousState;
    private readonly Dictionary<Keys, ICommand> _releaseBindings = new();

    public void Update()
    {
        var state = Keyboard.GetState();

        foreach (var binding in _keyBindings)
            if (state.IsKeyDown(binding.Key))
                binding.Value.Execute();

        foreach (var binding in _pressBindings)
            if (state.IsKeyDown(binding.Key) && _previousState.IsKeyUp(binding.Key))
                binding.Value.Execute();

        foreach (var binding in _releaseBindings)
            if (state.IsKeyUp(binding.Key) && _previousState.IsKeyDown(binding.Key))
                binding.Value.Execute();

        _previousState = state;
    }

    public void RegisterCommand(Keys key, ICommand command)
    {
        _keyBindings[key] = command;
    }

    /// Registers a command that runs once each time the key goes down, instead of every frame while held.
    public void RegisterPressCommand(Keys key, ICommand command)
    {
        _pressBindings[key] = command;
    }

    /// <summary>
    ///     Registers a command that runs when the key goes up and was previously down.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="command"></param>
    public void RegisterReleaseCommand(Keys key, ICommand command)
    {
        _releaseBindings[key] = command;
    }

    public void RemoveCommand(Keys key)
    {
        _keyBindings.Remove(key);
    }
}