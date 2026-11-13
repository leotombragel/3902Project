using System;
using System.Collections.Generic;
using System.Linq;
using CSE3902Project.Game.Command;
using CSE3902Project.Game.Entity;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.State;

/// <summary>
///     Updates the active game state based on if the player died or completed the level
/// </summary>
public class PlayerStatusChecker
{
    private readonly Dictionary<Func<bool>, ICommand> _conditions;

    public PlayerStatusChecker(Player player, StateManager stateManager, GameplayState gameplayState,
        GraphicsDevice device)
    {
        _conditions = new Dictionary<Func<bool>, ICommand>
        {
            { () => player.IsDead, new ResetCommand(gameplayState) },
            { () => player.FinishedLevel, new VictoryCommand(new VictoryState(gameplayState, device, stateManager), stateManager) }
        };
    }

    public void Update()
    {
        foreach (var condition in _conditions.Where(condition => condition.Key()))
        {
            condition.Value.Execute();
            break;
        }
    }
}