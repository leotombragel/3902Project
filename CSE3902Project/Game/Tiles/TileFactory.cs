using System;
using System.Collections.Generic;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Tiles;

/// Creates the tile that matches a stage cell type.
public static class TileFactory
{
    public static ITile Create(
        StageCellType cellType,
        Vector2 position,
        List<ItemSpawn> poppedItems,
        PipeSignature pipeSignature = null)
    {
        ArgumentNullException.ThrowIfNull(poppedItems);

        return cellType switch
        {
            StageCellType.Air or StageCellType.DeathZone => null,
            StageCellType.Ground => new GroundTile(position),
            StageCellType.UnderGround => new UnderGroundTile(position),
            StageCellType.Breakable => new BrickTile(position),
            StageCellType.BreakableUnderGround => new BrickTile(position, new BlockContents(),
                TileSpriteFactory.Instance.CreateBrickSprite(new Color(70, 90, 125))),
            StageCellType.BreakableWithCoin => new BrickTile(position,
                new BlockContents(StageCellType.RupeeItem, poppedItems)),
            StageCellType.BreakableWithBomb => new BrickTile(position,
                new BlockContents(StageCellType.BombItem, poppedItems)),
            StageCellType.Stair => new StepTile(position),
            StageCellType.QuestionBlock => new QuestionBlockTile(position,
                new BlockContents(StageCellType.RupeeItem, poppedItems)),
            StageCellType.QuestionBlockWithBoomerang => new QuestionBlockTile(position,
                new BlockContents(StageCellType.BoomerangItem, poppedItems)),
            StageCellType.HiddenBlockWithFire => new QuestionBlockTile(position),
            StageCellType.PipeLeftUpperBody =>
                new PipeTile(position, TileSpriteFactory.Instance.CreatePipeLeftUpperBodySprite()),
            StageCellType.PipeRightUpperBody =>
                new PipeTile(position, TileSpriteFactory.Instance.CreatePipeRightUpperBodySprite()),
            StageCellType.PipeLeftUpperOpening =>
                new PipeTile(position, TileSpriteFactory.Instance.CreatePipeLeftUpperOpeningSprite()),
            StageCellType.PipeRightUpperOpening =>
                new PipeTile(position, TileSpriteFactory.Instance.CreatePipeRightUpperOpeningSprite()),
            StageCellType.PipeLeftUpperConnectingBody =>
                new PipeTile(position, TileSpriteFactory.Instance.CreatePipeLeftUpperConnectingBodySprite()),
            StageCellType.PipeLeftLowerConnectingBody =>
                new PipeTile(position, TileSpriteFactory.Instance.CreatePipeLeftLowerConnectingBodySprite()),
            StageCellType.PipeLeftFacingUpperBody =>
                new PipeTile(position, TileSpriteFactory.Instance.CreatePipeLeftFacingUpperBodySprite()),
            StageCellType.PipeLeftFacingLowerBody =>
                new PipeTile(position, TileSpriteFactory.Instance.CreatePipeLeftFacingLowerBodySprite()),
            StageCellType.PipeLeftFacingUpperOpening =>
                new PipeTile(position, TileSpriteFactory.Instance.CreatePipeLeftFacingUpperOpeningSprite()),
            StageCellType.PipeLeftFacingLowerOpening =>
                new PipeTile(position, TileSpriteFactory.Instance.CreatePipeLeftFacingLowerOpeningSprite()),
            StageCellType.PipeEntrance => pipeSignature is null
                ? throw new ArgumentNullException(nameof(pipeSignature),
                    "A pipe entrance requires its parsed signature.")
                : new PipeEntranceTile(position, pipeSignature),
            _ => throw new ArgumentOutOfRangeException(nameof(cellType), cellType, "Unknown stage cell type.")
        };
    }
}
