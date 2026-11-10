using System;
using System.Globalization;

namespace CSE3902Project.Game.Tiles;

public sealed record PipeSignature(
    int Width,
    int Height,
    PipeDirection EntranceDirection,
    int ExitColumn,
    int ExitRow,
    int ExitWidth,
    int ExitHeight,
    PipeDirection ExitDirection,
    bool HasExitAnimation)
{
    public static PipeSignature Parse(string token)
    {
        ArgumentNullException.ThrowIfNull(token);
        var fields = token.Split('|');
        if (fields.Length != 9 ||
            !fields[0].Trim().Equals("pipestart", StringComparison.OrdinalIgnoreCase))
        {
            throw new FormatException("A pipe signature must have the form " +
                                      "pipestart|width|height|entranceDirection|column:row|" +
                                      "exitWidth|exitHeight|exitDirection|Y/N.");
        }

        var width = ParsePositiveInt(fields[1], "width");
        var height = ParsePositiveInt(fields[2], "height");
        var entranceDirection = ParseDirection(fields[3], "entrance direction");

        var exitCell = fields[4].Split(':');
        if (exitCell.Length != 2)
        {
            throw new FormatException("The pipe exit cell must use the column:row format.");
        }

        var exitColumn = ParseNonNegativeInt(exitCell[0], "exit column");
        var exitRow = ParseNonNegativeInt(exitCell[1], "exit row");
        var exitWidth = ParsePositiveInt(fields[5], "exit width");
        var exitHeight = ParsePositiveInt(fields[6], "exit height");
        var exitDirection = ParseDirection(fields[7], "exit direction");
        var hasExitAnimation = fields[8].Trim().ToUpperInvariant() switch
        {
            "Y" => true,
            "N" => false,
            _ => throw new FormatException("The exit animation value must be Y or N.")
        };

        return new PipeSignature(width, height, entranceDirection, exitColumn, exitRow,
            exitWidth, exitHeight, exitDirection, hasExitAnimation);
    }

    private static int ParsePositiveInt(string value, string name)
    {
        var parsed = ParseNonNegativeInt(value, name);
        if (parsed == 0) throw new FormatException($"Pipe {name} must be greater than zero.");
        return parsed;
    }

    private static int ParseNonNegativeInt(string value, string name)
    {
        if (!int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var parsed) || parsed < 0)
        {
            throw new FormatException($"Pipe {name} must be a non-negative integer.");
        }

        return parsed;
    }

    private static PipeDirection ParseDirection(string value, string name)
    {
        return value.Trim().ToLowerInvariant() switch
        {
            "left" => PipeDirection.Left,
            "right" => PipeDirection.Right,
            "down" => PipeDirection.Down,
            "up" => PipeDirection.Up,
            _ => throw new FormatException($"Pipe {name} must be left, right, down, or up.")
        };
    }
}
