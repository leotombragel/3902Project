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
    private AquamentusEnemy _aquamentusEnemy;
    private Background _background;
    private Camera _camera;
    private CameraFollowPlayer _cameraFollowPlayer;
    private CollisionSystem _collisionSystem;
    private List<IController> _controllers;
    private GraphicsDeviceManager _graphics;
    private Stage _stage;
    private ItemCycler _items;
    private KeeseEnemy _keeseEnemy;
    private Player _player;
    private SpriteBatch _spriteBatch;
    private StalfoEnemy _stalfoEnemy;
    private TileCycler _tiles;
    private WizzrobeEnemy _wizzrobeEnemy;

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
        _items = new ItemCycler(new Vector2(60, 48));
        _tiles = new TileCycler(new Vector2(92, 32));

        _player = new Player(_stage.GroundY);
        _player.SetStageCollision(new PlayerStageCollision(_stage.SolidTiles, _stage.Bounds));

        _camera = new Camera(viewport.Width, viewport.Height);
        _cameraFollowPlayer = new CameraFollowPlayer(_camera, _player, _stage.Bounds);

        _keeseEnemy = new KeeseEnemy(_stage.GroundY);
        _stalfoEnemy = new StalfoEnemy(_stage.GroundY);
        _wizzrobeEnemy = new WizzrobeEnemy(_player, _projectiles);
        _aquamentusEnemy = new AquamentusEnemy(_player, _projectiles);

        _enemies.Clear();
        _projectiles.Clear();
        _actors.Clear();
        _damageSources.Clear();

        _enemies.Add(_keeseEnemy);
        _enemies.Add(_stalfoEnemy);
        _enemies.Add(_wizzrobeEnemy);
        _enemies.Add(_aquamentusEnemy);
        _actors.Add(_player);
        _actors.AddRange(_enemies);



        // Bind commands to button presses
        keyboardController.RegisterCommand(Keys.D, new PlayerMoveRightCommand(_player));
        keyboardController.RegisterCommand(Keys.A, new PlayerMoveLeftCommand(_player));
        keyboardController.RegisterCommand(Keys.Space, new PlayerJumpCommand(_player));
        keyboardController.RegisterCommand(Keys.N, new PlayerAttackCommand(_player, attack => _attacks.Add(attack)));
        keyboardController.RegisterCommand(Keys.Z, new PlayerAttackCommand(_player, attack => _attacks.Add(attack)));
        keyboardController.RegisterCommand(Keys.E, new PlayerDamageCommand(_player));
        keyboardController.RegisterPressCommand(Keys.D1,
            new PlayerThrowBoomerangCommand(_player, projectile => _projectiles.Add(projectile)));
        keyboardController.RegisterPressCommand(Keys.D2,
            new PlayerThrowBombCommand(_player, projectile => _projectiles.Add(projectile)));
        keyboardController.RegisterPressCommand(Keys.D3,
            new PlayerShootFireCommand(_player, projectile => _projectiles.Add(projectile)));
        keyboardController.RegisterPressCommand(Keys.Q, new ExitCommand(this));
        keyboardController.RegisterPressCommand(Keys.R, new ResetCommand(this));
        keyboardController.RegisterPressCommand(Keys.T, new PreviousTileCommand(_tiles));
        keyboardController.RegisterPressCommand(Keys.Y, new NextTileCommand(_tiles));
        keyboardController.RegisterPressCommand(Keys.U, new PreviousItemCommand(_items));
        keyboardController.RegisterPressCommand(Keys.I, new NextItemCommand(_items));

        mouseController.RegisterCommand(MouseButton.LeftButton, new SetRockingPlayerSpriteCommand(_player));

        _controllers = new List<IController> { keyboardController, mouseController };
    }

    protected override void Update(GameTime gameTime)
    {
        foreach (var controller in _controllers) controller.Update();
        foreach (var attack in _attacks) attack.Update(gameTime);

        _player.Update(gameTime);
        _cameraFollowPlayer.Update();
        _stage.Update(gameTime);
        _tiles.Update(gameTime);
        _items.Update(gameTime);
        foreach (var projectile in _projectiles) projectile.Update(gameTime);
        _projectiles.RemoveAll(projectile => projectile.IsFinished);
        foreach (var enemy in _enemies) enemy.Update(gameTime);
        _enemies.RemoveAll(enemy => enemy.IsDead);

        _damageSources.AddRange(_projectiles);
        _damageSources.AddRange(_attacks);

        _collisionSystem.Update(_actors, _damageSources);

        _projectiles.RemoveAll(projectile => projectile.IsFinished);
        _attacks.RemoveAll(attack => attack.IsFinished);
        _enemies.RemoveAll(enemy => enemy.IsDead);
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
        _items.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}