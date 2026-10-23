using System;
using System.Collections.Generic;
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

namespace CSE3902Project;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private readonly List<ICombatActor> _actors = new();
    private readonly List<IAttack> _attacks = new();
    private readonly List<IDamageSource> _damageSources = new();

    private readonly List<IEnemy> _enemies = new();
    private readonly List<IProjectile> _projectiles = new();
    private Background _background;
    private Camera _camera;
    private CameraFollowPlayer _cameraFollowPlayer;
    private CollisionSystem _collisionSystem;
    private List<IController> _controllers;
    private GraphicsDeviceManager _graphics;
    private List<IItem> _items = new();
    private Player _player;
    private SpriteBatch _spriteBatch;
    private Stage _stage;
    private TileCycler _tiles;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice); // Texture rendering

        SpriteFactory.Instance.LoadAllAssets(Content); // Player sprites
        TileSpriteFactory.Instance.LoadAllAssets(Content); // Tile sprites
        ItemSpriteFactory.Instance.LoadAllAssets(Content); // Item sprites
        BackgroundSpriteFactory.Instance.LoadAllAssets(Content); // Background image

        _collisionSystem = new CollisionSystem(GraphicsDevice);

        ResetGame();
    }

    public void ResetGame()
    {
        var keyboardController = new KeyboardController();
        var mouseController = new MouseController();

        var viewport = GraphicsDevice.Viewport;
        _background = new Background(viewport.Width, viewport.Height);
        _stage = new Stage(viewport.Height);
        _items = Item.GetAllItems(new Vector2(300, 360));
        _tiles = new TileCycler(new Vector2(92, 32));

        _player = new Player(_stage.GroundY);
        _player.Position = _stage.PlayerSpawnPosition;
        _player.SetStageCollision(new PlayerStageCollision(_stage.SolidTiles, _stage.Bounds));

        _camera = new Camera(viewport.Width, viewport.Height);
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

        keyboardController.RegisterPressCommand(Keys.K, new PlayerBowDrawCommand(_player));
        keyboardController.RegisterReleaseCommand(Keys.K,
            new PlayerBowReleaseCommand(_player, projectile => _projectiles.Add(projectile)));

        var boomerangCommand = new PlayerThrowBoomerangCommand(_player, projectile => _projectiles.Add(projectile));
        var bombCommand = new PlayerThrowBombCommand(_player, projectile => _projectiles.Add(projectile));
        var fireCommand = new PlayerShootFireCommand(_player, projectile => _projectiles.Add(projectile));
        var currentItemCommand = new PlayerUseCurrentItemCommand(_player, [boomerangCommand, bombCommand, fireCommand]);

        keyboardController.RegisterPressCommand(Keys.D1, boomerangCommand);
        keyboardController.RegisterPressCommand(Keys.D2, bombCommand);
        keyboardController.RegisterPressCommand(Keys.D3, fireCommand);
        keyboardController.RegisterPressCommand(Keys.D4, currentItemCommand);

        keyboardController.RegisterPressCommand(Keys.Q, new ExitCommand(this));
        keyboardController.RegisterPressCommand(Keys.R, new ResetCommand(this));
        keyboardController.RegisterPressCommand(Keys.T, new PreviousTileCommand(_tiles));
        keyboardController.RegisterPressCommand(Keys.Y, new NextTileCommand(_tiles));

        mouseController.RegisterCommand(MouseButton.LeftButton, new SetRockingPlayerSpriteCommand(_player));

        _controllers = new List<IController> { keyboardController, mouseController };
    }

    private IEnemy CreateEnemy(EnemySpawn spawn)
    {
        return spawn.Type switch
        {
            StageCellType.Aquamentus => new AquamentusEnemy(_player, _projectiles, spawn.Position),
            StageCellType.Keese => new KeeseEnemy(_stage.GroundY, spawn.Position),
            StageCellType.Stalfo => new StalfoEnemy(spawn.Position),
            StageCellType.Wizzrobe => new WizzrobeEnemy(_player, _projectiles, spawn.Position),
            _ => throw new ArgumentOutOfRangeException(nameof(spawn), spawn.Type, "Unsupported enemy spawn type.")
        };
    }

    protected override void Update(GameTime gameTime)
    {
        foreach (var controller in _controllers) controller.Update();
        _player.Update(gameTime);
        foreach (var attack in _attacks) attack.Update(gameTime);

        _cameraFollowPlayer.Update();
        _stage.Update(gameTime);
        _tiles.Update(gameTime);
        foreach (var item in _items) item.Update(gameTime);
        foreach (var projectile in _projectiles) projectile.Update(gameTime);
        foreach (var enemy in _enemies) enemy.Update(gameTime);

        _damageSources.Clear();
        _damageSources.AddRange(_projectiles);
        _damageSources.AddRange(_attacks);

        _collisionSystem.Update(_actors, _damageSources);

        _projectiles.RemoveAll(projectile => projectile.IsFinished);
        _attacks.RemoveAll(attack => attack.IsFinished);
        _enemies.RemoveAll(enemy => enemy.IsDead);
        _items.RemoveAll(item => item.IsDead);
        _damageSources.RemoveAll(damageSource => damageSource.IsFinished);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);

        //background stays still on the screen
        _spriteBatch.Begin();
        _background.Draw(_spriteBatch);
        _spriteBatch.End();

        //everything in the stage gets moved by the camera
        _spriteBatch.Begin(transformMatrix: _camera.GetTransform());
        _stage.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);
        foreach (var projectile in _projectiles) projectile.Draw(_spriteBatch);
        foreach (var enemy in _enemies) enemy.Draw(_spriteBatch);
        _collisionSystem.Draw(_spriteBatch, _actors, _damageSources);
        _spriteBatch.End();

        //tile and item display stays in the top left corner
        _spriteBatch.Begin();
        _tiles.Draw(_spriteBatch);
        foreach (var item in _items) item.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}