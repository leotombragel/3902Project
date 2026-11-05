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
            ["gug"] = StageCellType.UnderGround,
            ["b"] = StageCellType.Breakable,
            ["bug"] = StageCellType.BreakableUnderGround,
            ["s"] = StageCellType.Stair,
            ["bc"] = StageCellType.BreakableWithCoin,
            ["bb"] = StageCellType.BreakableWithBomb,
            ["q"] = StageCellType.QuestionBlock,
            ["qb"] = StageCellType.QuestionBlockWithBoomerang,
            ["hb"] = StageCellType.HiddenBlockWithFire,
            ["pipelb"] = StageCellType.PipeLeftUpperBody,
            ["piperd"] = StageCellType.PipeRightUpperBody,
            ["piperb"] = StageCellType.PipeRightUpperBody,
            ["pipelo"] = StageCellType.PipeLeftUpperOpening,
            ["pipero"] = StageCellType.PipeRightUpperOpening,
            ["pipelbuc"] = StageCellType.PipeLeftUpperConnectingBody,
            ["pipelblc"] = StageCellType.PipeLeftLowerConnectingBody,
            ["pipelfub"] = StageCellType.PipeLeftFacingUpperBody,
            ["pipelflb"] = StageCellType.PipeLeftFacingLowerBody,
            ["pipelfuo"] = StageCellType.PipeLeftFacingUpperOpening,
            ["pipelflo"] = StageCellType.PipeLeftFacingLowerOpening,
            ["aq"] = StageCellType.Aquamentus,
            ["k"] = StageCellType.Keese,
            ["se"] = StageCellType.Stalfo,
            ["wr"] = StageCellType.Wizzrobe,
            ["wm"] = StageCellType.WallMaster,
            ["bmi"] = StageCellType.BoomerangItem,
            ["bwi"] = StageCellType.BowItem,
            ["ari"] = StageCellType.ArrowItem,
            ["bbi"] = StageCellType.BombItem,
            ["rui"] = StageCellType.RupeeItem,
            ["tri"] = StageCellType.TriforceItem,
            ["kyi"] = StageCellType.KeyItem,
            ["hri"] = StageCellType.HeartItem,
            ["spawn"] = StageCellType.PlayerSpawn,
            ["dz"] = StageCellType.DeathZone
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
