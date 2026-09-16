using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Sprint0_2.Game.Command;

namespace Sprint0_2.Game.Input;

public class MouseController : IController
{
    private Dictionary<MouseButton, ICommand> _mouseBindings = new();
    
    public void RegisterCommand(MouseButton button, ICommand command)
    {
        _mouseBindings[button] = command;
    }

    public void RemoveCommand(MouseButton button)
    {
        _mouseBindings.Remove(button);
    }

    public void Update()
    {
        MouseState ms = Mouse.GetState();
        
        foreach (var binding in _mouseBindings)
        {
            switch (binding.Key)
            {
                case MouseButton.LeftButton:
                    if (ms.LeftButton == ButtonState.Pressed)
                    {
                        binding.Value.Execute();
                    }
                    break;
                case MouseButton.RightButton:
                    if (ms.RightButton == ButtonState.Pressed)
                    {
                        binding.Value.Execute();
                    }
                    break;
                case MouseButton.MiddleButton:
                    if (ms.MiddleButton == ButtonState.Pressed)
                    {
                        binding.Value.Execute();
                    }
                    break;
            }
        }
    }
}