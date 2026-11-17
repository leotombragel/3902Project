using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Tiles;

/// The level the player walks around in.
public class Stage
{
    private const int LegacyPipeEntranceColumn = 46;
    private const int LegacyPipeEntranceRow = 6;

    private static readonly PipeSignature LegacyPipeSignature =
        new(2, 1, PipeDirection.Down, 236, 0, 1, 1, PipeDirection.Up, false);

    private readonly int _mapTop;

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
        _mapTop = mapTop;
        Bounds = new Rectangle(0, 0, mapWidthInTiles * Tile.Size, Math.Max(screenHeight, mapHeight));
        SolidTiles = new List<ITile>();
        var cameraBoundsMarkers = new Dictionary<string, List<Point>>(StringComparer.Ordinal);
        GroundY = LoadStageFromCsv(mapTokens, mapTop, cameraBoundsMarkers);
        CameraBounds = BuildCameraBounds(cameraBoundsMarkers);
        if (PipeEntrances.Count == 0)
            PipeEntrances.Add((PipeEntranceTile)TileFactory.Create(
                StageCellType.PipeEntrance,
                new Vector2(LegacyPipeEntranceColumn * Tile.Size, mapTop + LegacyPipeEntranceRow * Tile.Size),
                PoppedItems,
                LegacyPipeSignature));

        BuildSolidRectangles();
    }

    public int GroundY { get; }
    public Rectangle Bounds { get; }
    public IReadOnlyList<CameraBoundsArea> CameraBounds { get; }
    public List<PipeEntranceTile> PipeEntrances { get; } = new();

    public List<Rectangle> PipeEntranceBounds =>
        PipeEntrances.Select(entrance => entrance.InteractionBounds).ToList();

    public List<Rectangle> PipeExitBounds =>
        PipeEntrances.Select(GetPipeExitBounds).ToList();

    public List<ITile> SolidTiles { get; }
    public List<Rectangle> SolidRectangles { get; } = new();
    public List<EnemySpawn> EnemySpawns { get; } = new();
    public List<ItemSpawn> ItemSpawns { get; } = new();

    //items that popped out of blocks, the game turns these into real items
    public List<ItemSpawn> PoppedItems { get; } = new();
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

    private int LoadStageFromCsv(
        string[][] mapTokens,
        int mapTop,
        Dictionary<string, List<Point>> cameraBoundsMarkers)
    {
        var groundY = -1;
        var hasPlayerSpawn = false;
        for (var row = 0; row < mapTokens.Length; row++)
        for (var column = 0; column < mapTokens[row].Length; column++)
        {
            var position = new Vector2(column * Tile.Size, mapTop + row * Tile.Size);
            var cellTokens = mapTokens[row][column].Split(new[] { "//" }, StringSplitOptions.None);
            foreach (var cellToken in cellTokens)
                LoadStageCellToken(
                    cellToken.Trim().TrimStart('\uFEFF'),
                    row,
                    column,
                    position,
                    cameraBoundsMarkers,
                    ref groundY,
                    ref hasPlayerSpawn);
        }

        if (groundY < 0) throw new InvalidDataException("The stage CSV does not contain any ground tiles.");

        if (!hasPlayerSpawn) throw new InvalidDataException("The stage CSV does not contain a player spawn.");

        return groundY;
    }

    private void LoadStageCellToken(
        string token,
        int row,
        int column,
        Vector2 position,
        Dictionary<string, List<Point>> cameraBoundsMarkers,
        ref int groundY,
        ref bool hasPlayerSpawn)
    {
        if (!StageCellMap.TryGetCellType(token, out var cellType))
        {
            Console.WriteLine(
                $"Unknown stage token '{token}' at row {row + 1}, column {column + 1}; treating it as air.");
            return;
        }

        if (cellType == StageCellType.CameraBoundsMarker)
        {
            string id;
            try
            {
                id = StageCellMap.ParseCameraBoundsId(token);
            }
            catch (FormatException exception)
            {
                throw new InvalidDataException(
                    $"Invalid camera bounds marker at row {row + 1}, column {column + 1}: {exception.Message}",
                    exception);
            }

            if (!cameraBoundsMarkers.TryGetValue(id, out var markers))
            {
                markers = new List<Point>();
                cameraBoundsMarkers.Add(id, markers);
            }

            markers.Add(new Point((int)position.X, (int)position.Y));
            return;
        }

        if (cellType == StageCellType.PipeEntrance)
        {
            try
            {
                PipeEntrances.Add((PipeEntranceTile)CreateTile(
                    cellType,
                    position,
                    PipeSignature.Parse(token)));
            }
            catch (FormatException exception)
            {
                throw new InvalidDataException(
                    $"Invalid pipe signature at row {row + 1}, column {column + 1}: {exception.Message}",
                    exception);
            }

            return;
        }

        if (cellType == StageCellType.PlayerSpawn)
        {
            PlayerSpawnPosition = position;
            hasPlayerSpawn = true;
            return;
        }

        if (IsEnemy(cellType))
        {
            EnemySpawns.Add(new EnemySpawn(cellType, position));
            return;
        }

        if (isItem(cellType))
        {
            ItemSpawns.Add(new ItemSpawn(cellType, position));
            return;
        }

        var tile = CreateTile(cellType, position);
        if (tile is null) return;

        SolidTiles.Add(tile);
        if (cellType is StageCellType.Ground or StageCellType.UnderGround && groundY < 0)
            groundY = (int)position.Y;
    }

    private static IReadOnlyList<CameraBoundsArea> BuildCameraBounds(
        Dictionary<string, List<Point>> cameraBoundsMarkers)
    {
        var areas = new List<CameraBoundsArea>();
        foreach (var (id, markers) in cameraBoundsMarkers)
        {
            if (markers.Count != 2)
                throw new InvalidDataException(
                    $"Camera bounds area '{id}' must have exactly two corner markers; found {markers.Count}.");

            var left = Math.Min(markers[0].X, markers[1].X);
            var top = Math.Min(markers[0].Y, markers[1].Y);
            var right = Math.Max(markers[0].X, markers[1].X) + Tile.Size;
            var bottom = Math.Max(markers[0].Y, markers[1].Y) + Tile.Size;
            var area = new CameraBoundsArea(id, new Rectangle(left, top, right - left, bottom - top));

            var overlappingArea = areas.FirstOrDefault(existing => existing.Bounds.Intersects(area.Bounds));
            if (overlappingArea.Id is not null)
                throw new InvalidDataException(
                    $"Camera bounds areas '{overlappingArea.Id}' and '{id}' overlap.");

            areas.Add(area);
        }

        return areas;
    }

    public Vector2 GetPipeExitPosition(PipeEntranceTile entrance)
    {
        ArgumentNullException.ThrowIfNull(entrance);
        return new Vector2(
            entrance.Signature.ExitColumn * Tile.Size,
            _mapTop + entrance.Signature.ExitRow * Tile.Size);
    }

    private Rectangle GetPipeExitBounds(PipeEntranceTile entrance)
    {
        var position = GetPipeExitPosition(entrance);
        return new Rectangle(
            (int)position.X,
            (int)position.Y,
            entrance.Signature.ExitWidth * Tile.Size,
            entrance.Signature.ExitHeight * Tile.Size);
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
            or StageCellType.HeartItem
            or StageCellType.TorchItem;
    }

    private ITile CreateTile(StageCellType cellType, Vector2 position, PipeSignature pipeSignature = null)
    {
        return TileFactory.Create(cellType, position, PoppedItems, pipeSignature);
    }

    //removes broken bricks from both lists
    private void RemoveBrokenTiles()
    {
        var removedAny = false;

        for (var i = SolidTiles.Count - 1; i >= 0; i--)
            if (SolidTiles[i].IsBroken)
            {
                SolidTiles.RemoveAt(i);
                removedAny = true;
            }

        if (removedAny) BuildSolidRectangles();
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