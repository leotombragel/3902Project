using System;
using System.Collections.Generic;

namespace CSE3902Project.Game.Tiles;

public static class StageCellMap
{
    private static readonly IReadOnlyDictionary<string, StageCellType> CellTypes =
        new Dictionary<string, StageCellType>(StringComparer.Ordinal)
        {
            [""] = StageCellType.Air,
            ["g"] = StageCellType.Ground,
            ["b"] = StageCellType.Breakable,
            ["s"] = StageCellType.Stair,
            ["bc"] = StageCellType.BreakableWithCoin,
            ["bb"] = StageCellType.BreakableWithBomb,
            ["q"] = StageCellType.QuestionBlock,
            ["qb"] = StageCellType.QuestionBlockWithBoomerang,
            ["hb"] = StageCellType.HiddenBlockWithFire,
            ["pipelb"] = StageCellType.PipeLeftBody,
            ["piperd"] = StageCellType.PipeRightBody,
            ["piperb"] = StageCellType.PipeRightBody,
            ["pipelo"] = StageCellType.PipeLeftOpening,
            ["pipero"] = StageCellType.PipeRightOpening,
            ["aq"] = StageCellType.Aquamentus,
            ["k"] = StageCellType.Keese,
            ["se"] = StageCellType.Stalfo,
            ["wr"] = StageCellType.Wizzrobe,
            ["wm"] = StageCellType.WallMaster,
            ["spawn"] = StageCellType.PlayerSpawn
        };

    public static bool TryGetCellType(string token, out StageCellType cellType)
    {
        if (token is null)
        {
            cellType = default;
            return false;
        }

        return CellTypes.TryGetValue(token.Trim().TrimStart('\uFEFF'), out cellType);
    }
}
