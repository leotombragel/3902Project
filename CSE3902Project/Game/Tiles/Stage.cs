using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Tiles;

/// The level the player walks around in.
public class Stage
{
    public int GroundY { get; }
    public Rectangle Bounds { get; }
    public List<ITile> SolidTiles { get; }
    public List<EnemySpawn> EnemySpawns { get; } = new();
    public string[] MapRows { get; }
    public Vector2 PlayerSpawnPosition { get; private set; }

    public Stage(int screenHeight)
    {
        var mapPath = Path.Combine(AppContext.BaseDirectory, "Content", "levelFiles", "1-1LevelExcel.csv");
        var mapRows = File.ReadAllLines(mapPath);
        var mapRowCount = mapRows.Length;
        while (mapRowCount > 0 && string.IsNullOrWhiteSpace(mapRows[mapRowCount - 1]))
        {
            mapRowCount--;
        }

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
    }

    public void Update(GameTime gameTime)
    {
        foreach (var tile in SolidTiles)
        {
            tile.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var tile in SolidTiles)
        {
            tile.Draw(spriteBatch);
        }
    }

    private int LoadStageFromCsv(string[][] mapTokens, int mapTop)
    {
        var groundY = -1;
        var hasPlayerSpawn = false;
        for (var row = 0; row < mapTokens.Length; row++)
        {
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

                var tile = CreateTile(cellType, position);
                if (tile is null)
                {
                    continue;
                }

                SolidTiles.Add(tile);
                if (cellType == StageCellType.Ground && groundY < 0)
                {
                    groundY = (int)position.Y;
                }
            }
        }

        if (groundY < 0)
        {
            throw new InvalidDataException("The stage CSV does not contain any ground tiles.");
        }

        if (!hasPlayerSpawn)
        {
            throw new InvalidDataException("The stage CSV does not contain a player spawn.");
        }

        return groundY;
    }

    private static bool IsEnemy(StageCellType cellType)
    {
        return cellType is StageCellType.Aquamentus
            or StageCellType.Keese
            or StageCellType.Stalfo
            or StageCellType.Wizzrobe;
    }

    private static ITile CreateTile(StageCellType cellType, Vector2 position)
    {
        switch (cellType)
        {
            case StageCellType.Ground:
                return TileFactory.Create(TileType.Ground, position);
            case StageCellType.Breakable:
                return TileFactory.Create(TileType.Brick, position);
            case StageCellType.BreakableWithCoin:
                return TileFactory.Create(TileType.Brick, position);
            case StageCellType.BreakableWithBomb:
                return TileFactory.Create(TileType.Brick, position);
            case StageCellType.Stair:
                return TileFactory.Create(TileType.Step, position);
            case StageCellType.QuestionBlock:
                return TileFactory.Create(TileType.QuestionBlock, position);
            case StageCellType.QuestionBlockWithBoomerang:
                return TileFactory.Create(TileType.QuestionBlock, position);
            case StageCellType.HiddenBlockWithFire:
                return TileFactory.Create(TileType.QuestionBlock, position);
            case StageCellType.PipeLeftBody:
                return TileFactory.Create(TileType.Pipe, position);
            case StageCellType.PipeRightBody:
                return TileFactory.Create(TileType.Pipe, position);
            case StageCellType.PipeLeftOpening:
                return TileFactory.Create(TileType.Pipe, position);
            case StageCellType.PipeRightOpening:
                return TileFactory.Create(TileType.Pipe, position);
            case StageCellType.Air:
                return null;
            default:
                throw new ArgumentOutOfRangeException(nameof(cellType), cellType, "Unknown stage cell type.");
        }
    }
}
