using System;
using System.Collections.Generic;
using System.Linq;
using CSE3902Project.Game.Cameras;
using CSE3902Project.Game.Collision;
using CSE3902Project.Game.Command;
using CSE3902Project.Game.Damage;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using CSE3902Project.Game.Input;
using CSE3902Project.Game.Items;
using CSE3902Project.Game.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CSE3902Project.Game.State;

public class GameplayState : IGameState
{
    // Stage element collections
    private readonly List<ICombatActor> _actors = new();
    private readonly List<IAttack> _attacks = new();
    private readonly CollisionSystem _collisionSystem;
    private readonly List<IDamageSource> _damageSources = new();
    private readonly List<IEnemy> _enemies = new();
    private readonly Game1 _game;
    private readonly GraphicsDevice _graphicsDevice;

    // Other
    private readonly PauseState _pauseState;
    private readonly List<IProjectile> _projectiles = new();
    private readonly StateManager _stateManager;
    private Background _background;
    private readonly MusicFactory _backgroundMusic = MusicFactory.Instance;

    // Other systems in the stage
    private Camera _camera;
    private CameraFollowPlayer _cameraFollowPlayer;

    private List<IController> _controllers;
    private Hud.Hud _hud;
    private List<IItem> _items = new();

    // Standalone stage elements
    private Player _player;
    private bool _showHitboxes;
    private Stage _stage;

    public GameplayState(GraphicsDevice graphics, StateManager stateManager, Game1 game)
    {
        _stateManager = stateManager;
        _graphicsDevice = graphics;
        _game = game;

        _collisionSystem = new CollisionSystem(graphics);
        _pauseState = new PauseState(graphics, stateManager, game);
        ResetGame();
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update(GameTime gameTime)
    {
        foreach (var controller in _controllers) controller.Update();
        _player.Update(gameTime);
        foreach (var attack in _attacks) attack.Update(gameTime);

        _cameraFollowPlayer.Update();
        _hud.Update(gameTime);
        _stage.Update(gameTime);

        //turn the items that popped out into real items
        foreach (var spawn in _stage.PoppedItems)
        {
            var poppedItem = CreateItem(spawn);
            _items.Add(poppedItem);
            _actors.Add(poppedItem);
        }

        _stage.PoppedItems.Clear();

        foreach (var item in _items) item.Update(gameTime);
        foreach (var projectile in _projectiles) projectile.Update(gameTime);
        foreach (var enemy in _enemies) enemy.Update(gameTime);

        _damageSources.Clear();
        _damageSources.AddRange(_projectiles);
        _damageSources.AddRange(_attacks);

        _collisionSystem.Update(_actors, _damageSources, _stage.SolidRectangles);
        _collisionSystem.Update(_player, _enemies, _stage.SolidRectangles);

        _projectiles.RemoveAll(projectile => projectile.IsFinished);
        _attacks.RemoveAll(attack => attack.IsFinished);
        _enemies.RemoveAll(enemy => enemy.IsDead);
        _items.RemoveAll(item => item.IsDead);
        _damageSources.RemoveAll(damageSource => damageSource.IsFinished);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        //background stays still on the screen
        spriteBatch.Begin();
        _background.Draw(spriteBatch);
        spriteBatch.End();

        //everything in the stage gets moved by the camera
        spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetTransform());
        _stage.Draw(spriteBatch);
        _player.Draw(spriteBatch);
        foreach (var item in _items) item.Draw(spriteBatch);
        foreach (var projectile in _projectiles) projectile.Draw(spriteBatch);
        foreach (var enemy in _enemies) enemy.Draw(spriteBatch);

        if (_showHitboxes)
        {
            _collisionSystem.Draw(spriteBatch, _actors, _damageSources);
            _collisionSystem.DrawStageHitboxes(
                spriteBatch,
                _stage.SolidTiles,
                _player.StageHitbox,
                _stage.PipeEntranceBounds,
                _stage.PipeExitBounds);
        }

        spriteBatch.End();


        //hud stays still on the screen (point sampling keeps the pixel art sharp)
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _hud.Draw(spriteBatch);
        spriteBatch.End();
    }

    private void TryEnterPipe(PipeDirection inputDirection)
    {
        if (!_player.IsOnGround || _player.IsDead) return;

        var playerHitbox = _player.StageHitbox;
        var entrance = _stage.PipeEntrances
            .FirstOrDefault(pipe =>
                pipe.Signature.EntranceDirection == inputDirection &&
                playerHitbox.Intersects(pipe.InteractionBounds));
        if (entrance is null) return;

        _player.TeleportTo(_stage.GetPipeExitPosition(entrance));
    }

    public void ResetGame()
    {
        _backgroundMusic.PlayOverworldMusic();
        var keyboardController = new KeyboardController();
        var mouseController = new MouseController();

        var viewport = _graphicsDevice.Viewport;
        _background = new Background(viewport.Width, viewport.Height);
        _stage = new Stage(viewport.Height);
        _items = new List<IItem>();

        _player = new Player(_stage.GroundY);
        _player.Position = _stage.PlayerSpawnPosition;
        _player.SetStageCollision(new PlayerStageCollision(_stage.SolidTiles, _stage.Bounds));

        _camera = new Camera(viewport.Width, viewport.Height);
        _hud = new Hud.Hud(_player, viewport.Width);
        _cameraFollowPlayer = new CameraFollowPlayer(_camera, _player, _stage.Bounds);

        _enemies.Clear();
        _projectiles.Clear();
        _actors.Clear();
        _damageSources.Clear();

        _actors.Add(_player);
        foreach (var spawn in _stage.EnemySpawns)
        {
            var enemy = CreateEnemy(spawn);
            _enemies.Add(enemy);
            _actors.Add(enemy);
        }

        foreach (var item in _stage.ItemSpawns) _items.Add(CreateItem(item));

        _actors.AddRange(_items);

        // Bind commands to button presses
        keyboardController.RegisterCommand(Keys.D, new PlayerMoveRightCommand(_player));
        keyboardController.RegisterCommand(Keys.A, new PlayerMoveLeftCommand(_player));
        keyboardController.RegisterPressCommand(Keys.Space, new PlayerJumpCommand(_player));
        keyboardController.RegisterPressCommand(Keys.N,
            new PlayerAttackCommand(_player, attack => _attacks.Add(attack)));
        keyboardController.RegisterPressCommand(Keys.Z,
            new PlayerAttackCommand(_player, attack => _attacks.Add(attack)));
        keyboardController.RegisterPressCommand(Keys.E, new PlayerDamageCommand(_player));
        keyboardController.RegisterPressCommand(Keys.M, new ToggleMusicCommand(_backgroundMusic));
        keyboardController.RegisterCommand(Keys.S,
            new EnterPipeCommand(() => TryEnterPipe(PipeDirection.Down)));
        keyboardController.RegisterCommand(Keys.A,
            new EnterPipeCommand(() => TryEnterPipe(PipeDirection.Left)));
        keyboardController.RegisterCommand(Keys.D,
            new EnterPipeCommand(() => TryEnterPipe(PipeDirection.Right)));
        keyboardController.RegisterCommand(Keys.W,
            new EnterPipeCommand(() => TryEnterPipe(PipeDirection.Up)));
        keyboardController.RegisterPressCommand(Keys.F3,
            new ToggleHitboxesCommand(() => _showHitboxes = !_showHitboxes));

        var boomerangCommand = new PlayerThrowBoomerangCommand(_player, projectile => _projectiles.Add(projectile));
        var bombCommand = new PlayerThrowBombCommand(_player, projectile => _projectiles.Add(projectile));
        var fireCommand = new PlayerShootFireCommand(_player, projectile => _projectiles.Add(projectile));
        var bowDrawCommand = new PlayerBowDrawCommand(_player);
        //var bowDrawCommandNumber = new PlayerBowDrawCommand(_player, false);
        var bowReleaseCommand = new PlayerBowReleaseCommand(_player, projectile => _projectiles.Add(projectile));
        //var bowReleaseCommandNumber =
        //new PlayerBowReleaseCommand(_player, projectile => _projectiles.Add(projectile), false);
        var currentItemCommand =
            new PlayerUseCurrentItemCommand(_player, [boomerangCommand, bombCommand, fireCommand, bowDrawCommand]);

        keyboardController.RegisterPressCommand(Keys.K, currentItemCommand);
        keyboardController.RegisterReleaseCommand(Keys.K, bowReleaseCommand);
        //keyboardController.RegisterPressCommand(Keys.K, bowDrawCommand);

        //keyboardController.RegisterPressCommand(Keys.D1, boomerangCommand);
        //keyboardController.RegisterPressCommand(Keys.D2, bombCommand);
        //keyboardController.RegisterPressCommand(Keys.D3, fireCommand);
        //keyboardController.RegisterPressCommand(Keys.D4, bowDrawCommandNumber);
        //keyboardController.RegisterReleaseCommand(Keys.D4, bowReleaseCommandNumber);

        keyboardController.RegisterPressCommand(Keys.Q, new ExitCommand(_game));
        keyboardController.RegisterPressCommand(Keys.R, new ResetCommand(this));
        keyboardController.RegisterPressCommand(Keys.Escape, new PauseCommand(_pauseState, _stateManager));

        _controllers = new List<IController> { keyboardController, mouseController };
    }

    private IEnemy CreateEnemy(EnemySpawn spawn)
    {
        return spawn.Type switch
        {
            StageCellType.Aquamentus => new AquamentusEnemy(_player, _projectiles, spawn.Position),
            StageCellType.Keese => new KeeseEnemy(_stage.GroundY, spawn.Position),
            StageCellType.Stalfo => CreateStalfoEnemy(spawn.Position),
            StageCellType.Wizzrobe => CreateWizzrobeEnemy(_player, _projectiles, spawn.Position),
            StageCellType.WallMaster => CreateWallmasterEnemy(_player, spawn.Position),
            _ => throw new ArgumentOutOfRangeException(nameof(spawn), spawn.Type, "Unsupported enemy spawn type.")
        };
    }

    private StalfoEnemy CreateStalfoEnemy(Vector2 spawnPosition)
    {
        var stalfo = new StalfoEnemy(spawnPosition);
        stalfo.SetStageCollision(new StalfoStageCollision(_stage.SolidTiles));
        return stalfo;
    }

    private WizzrobeEnemy CreateWizzrobeEnemy(Player player, List<IProjectile> projectiles, Vector2 spawnPosition)
    {
        var wizzrobe = new WizzrobeEnemy(player, projectiles, spawnPosition);
        wizzrobe.SetCollision(new WizzrobeCollision(_stage.SolidTiles));
        return wizzrobe;
    }

    private WallmasterEnemy CreateWallmasterEnemy(Player player, Vector2 spawnPosition)
    {
        var wallmaster = new WallmasterEnemy(player, spawnPosition);
        wallmaster.SetStageCollision(new WallmasterStageCollision(_stage.SolidTiles));
        return wallmaster;
    }

    private static IItem CreateItem(ItemSpawn spawn)
    {
        var center = spawn.Position + new Vector2(Tile.Size / 2f);
        return spawn.Type switch
        {
            StageCellType.BoomerangItem => new BoomerangItem(center),
            StageCellType.BowItem => new BowItem(center),
            StageCellType.ArrowItem => new ArrowItem(center),
            StageCellType.BombItem => new BombItem(center),
            StageCellType.RupeeItem => new RupeeItem(center),
            StageCellType.TriforceItem => new TriforceItem(center),
            StageCellType.KeyItem => new KeyItem(center),
            StageCellType.HeartItem => new HeartItem(center),
            StageCellType.TorchItem => new TorchItem(center),
            _ => throw new ArgumentOutOfRangeException(nameof(spawn), spawn.Type, "Unsupported item spawn type.")
        };
    }
}