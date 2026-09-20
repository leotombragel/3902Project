using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using CSE3902Project.Game.Command;

namespace CSE3902Project.Game.Input;

public class KeyboardController : IController
{
    private Dictionary<Keys, ICommand> _keyBindings = new();
    
    public void RegisterCommand(Keys key, ICommand command)
    {
        _keyBindings[key] = command;
    }

    public void RemoveCommand(Keys key)
    {
        _keyBindings.Remove(key);
    }

    public void Update()
    {
        KeyboardState state = Keyboard.GetState();

        foreach (var binding in _keyBindings)
        {
            if (state.IsKeyDown(binding.Key))
            {
                binding.Value.Execute();
            }
        }
    }
}