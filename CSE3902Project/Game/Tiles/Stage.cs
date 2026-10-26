using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Tiles;

/// The level the player walks around in.
public class Stage
{
    public Stage(int screenHeight)
    {
        var mapPath = Path.Combine(AppContext.BaseDirectory, "Content", "levelFiles", "1-1LevelExcel.csv");
        var mapRows = File.ReadAllLines(mapPath);
        var mapRowCount = mapRows.Length;
        while (mapRowCount > 0 && string.IsNullOrWhiteSpace(mapRows[mapRowCount - 1])) mapRowCount--;

        MapRows = mapRows[..mapRowCount];
        var mapTokens = new string[MapRows.Length][];
        var mapWidthInTiles = 0;
        for (var row = 0; row < MapRows.Length; row++)
        {
            mapTokens[row] = MapRows[row].Split(',');
            mapWidthInTiles = Math.Max(mapWidthInTiles, mapTokens[row].Length);
        }

        var mapHeight = MapRows.Length * Tile.Size;
        var mapTop = Math.Max(0, screenHeight - mapHeight);
        Bounds = new Rectangle(0, 0, mapWidthInTiles * Tile.Size, Math.Max(screenHeight, mapHeight));
        SolidTiles = new List<ITile>();
        GroundY = LoadStageFromCsv(mapTokens, mapTop);
        BuildSolidRectangles();
    }

    public int GroundY { get; }
    public Rectangle Bounds { get; }
    public List<ITile> SolidTiles { get; }
    public List<Rectangle> SolidRectangles { get; } = new();
    public List<EnemySpawn> EnemySpawns { get; } = new();
    public List<ItemSpawn> ItemSpawns { get; } = new();

    //items that popped out of blocks, the game turns these into real items
    public List<ItemSpawn> PoppedItems { get; } = new List<ItemSpawn>();
    public string[] MapRows { get; }
    public Vector2 PlayerSpawnPosition { get; private set; }

    public void Update(GameTime gameTime)
    {
        foreach (var tile in SolidTiles) tile.Update(gameTime);
        RemoveBrokenTiles();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var tile in SolidTiles) tile.Draw(spriteBatch);
    }

    private int LoadStageFromCsv(string[][] mapTokens, int mapTop)
    {
        var groundY = -1;
        var hasPlayerSpawn = false;
        for (var row = 0; row < mapTokens.Length; row++)
        for (var column = 0; column < mapTokens[row].Length; column++)
        {
            var token = mapTokens[row][column].Trim().TrimStart('\uFEFF');
            if (!StageCellMap.TryGetCellType(token, out var cellType))
            {
                Console.WriteLine(
                    $"Unknown stage token '{token}' at row {row + 1}, column {column + 1}; treating it as air.");
                cellType = StageCellType.Air;
            }

            var position = new Vector2(column * Tile.Size, mapTop + row * Tile.Size);
            if (cellType == StageCellType.PlayerSpawn)
            {
                PlayerSpawnPosition = position;
                hasPlayerSpawn = true;
                continue;
            }

            if (IsEnemy(cellType))
            {
                EnemySpawns.Add(new EnemySpawn(cellType, position));
                continue;
            }

            if (isItem(cellType))
            {
                ItemSpawns.Add(new ItemSpawn(cellType, position));
                continue;
            }

            var tile = CreateTile(cellType, position);
            if (tile is null)
            {
                continue;
            }

            SolidTiles.Add(tile);
            if (cellType == StageCellType.Ground && groundY < 0) groundY = (int)position.Y;
        }

        if (groundY < 0) throw new InvalidDataException("The stage CSV does not contain any ground tiles.");

        if (!hasPlayerSpawn) throw new InvalidDataException("The stage CSV does not contain a player spawn.");

        return groundY;
    }

    private static bool IsEnemy(StageCellType cellType)
    {
        return cellType is StageCellType.Aquamentus
            or StageCellType.Keese
            or StageCellType.Stalfo
            or StageCellType.Wizzrobe
            or StageCellType.WallMaster;
    }

    private static bool isItem(StageCellType cellType)
    {
        return cellType is StageCellType.BoomerangItem
            or StageCellType.BowItem
            or StageCellType.ArrowItem
            or StageCellType.BombItem
            or StageCellType.RupeeItem
            or StageCellType.TriforceItem
            or StageCellType.KeyItem
            or StageCellType.HeartItem;
    }

    private ITile CreateTile(StageCellType cellType, Vector2 position)
    {
        switch (cellType)
        {
            case StageCellType.Ground:
                return TileFactory.Create(TileType.Ground, position);
            case StageCellType.Breakable:
                return TileFactory.Create(TileType.Brick, position);
            case StageCellType.BreakableWithCoin:
                return new BrickTile(position, new BlockContents(StageCellType.RupeeItem, PoppedItems));
            case StageCellType.BreakableWithBomb:
                return new BrickTile(position, new BlockContents(StageCellType.BombItem, PoppedItems));
            case StageCellType.Stair:
                return TileFactory.Create(TileType.Step, position);
            case StageCellType.QuestionBlock:
                return new QuestionBlockTile(position, new BlockContents(StageCellType.RupeeItem, PoppedItems));
            case StageCellType.QuestionBlockWithBoomerang:
                return new QuestionBlockTile(position,
                    new BlockContents(StageCellType.BoomerangItem, PoppedItems));
            case StageCellType.HiddenBlockWithFire:
                return TileFactory.Create(TileType.QuestionBlock, position);
            case StageCellType.PipeLeftBody:
                return new PipeTile(position, TileSpriteFactory.Instance.CreatePipeLeftBodySprite());
            case StageCellType.PipeRightBody:
                return new PipeTile(position, TileSpriteFactory.Instance.CreatePipeRightBodySprite());
            case StageCellType.PipeLeftOpening:
                return new PipeTile(position, TileSpriteFactory.Instance.CreatePipeLeftOpeningSprite());
            case StageCellType.PipeRightOpening:
                return new PipeTile(position, TileSpriteFactory.Instance.CreatePipeRightOpeningSprite());
            case StageCellType.Air:
                return null;
            default:
                throw new ArgumentOutOfRangeException(nameof(cellType), cellType, "Unknown stage cell type.");
        }
    }

    //removes broken bricks from both lists
    private void RemoveBrokenTiles()
    {
        bool removedAny = false;

        for (int i = SolidTiles.Count - 1; i >= 0; i--)
        {
            if (SolidTiles[i].IsBroken)
            {
                SolidTiles.RemoveAt(i);
                removedAny = true;
            }
        }

        if (removedAny)
        {
            BuildSolidRectangles();
        }
    }

    private void BuildSolidRectangles()
    {
        SolidRectangles.Clear();
        var rectangles = new List<Rectangle>();

        var grouped = SolidTiles
            .Select(tile => tile.Bounds)
            .OrderBy(r => r.Y)
            .ThenBy(r => r.X)
            .ToList();

        foreach (var rect in grouped)
        {
            if (rectangles.Count == 0)
            {
                rectangles.Add(rect);
                continue;
            }

            var lastRect = rectangles.Last();

            // Merge rectangles that are in the same row and adjacent horizontally
            if (lastRect.Y == rect.Y && lastRect.Right == rect.X)
            {
                rectangles[^1] = new Rectangle(lastRect.X, lastRect.Y, lastRect.Width + rect.Width, lastRect.Height);
                continue;
            }

            rectangles.Add(rect);
        }

        SolidRectangles.AddRange(rectangles);
    }
}