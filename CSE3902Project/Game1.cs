using System.Collections.Generic;
using CSE3902Project.Game.Command;
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
    private List<IController> _controllers;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Player _player;
    private TileCycler _tiles;
    private ItemCycler _items;
    private Background _background;
    private EnemyCycler _enemyCycler;
    private GroundRow _ground;
    private KeeseEnemy _keeseEnemy;
    private StalfoEnemy _stalfoEnemy;
    private AquamentusEnemy _aquamentusEnemy;
    private readonly List<IProjectile> _projectiles = new();
    private readonly List<IMortal> _enemies = new();
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

        ResetGame();
    }
    public void ResetGame()
    {
        var keyboardController = new KeyboardController();
        var mouseController = new MouseController();

        var viewport = GraphicsDevice.Viewport;
        _background = new Background(viewport.Width, viewport.Height);
        _ground = new GroundRow(viewport.Width, viewport.Height);
        _items = new ItemCycler(new Vector2(60, 48));
        _tiles = new TileCycler(new Vector2(92, 32));

        _player = new Player(_ground.TileHeight);

        _keeseEnemy = new KeeseEnemy(_ground.TileHeight);
        _stalfoEnemy = new StalfoEnemy(_ground.TileHeight);
        _wizzrobeEnemy = new WizzrobeEnemy(_player, _projectiles);
        _aquamentusEnemy = new AquamentusEnemy(_player, _projectiles);

        _enemies.Clear();
        _projectiles.Clear();

        _enemies.Add(_keeseEnemy);
        _enemies.Add(_stalfoEnemy);
        _enemies.Add(_wizzrobeEnemy);
        _enemies.Add(_aquamentusEnemy);

        _enemyCycler = new EnemyCycler(_enemies);
        

        // Bind commands to button presses
        keyboardController.RegisterCommand(Keys.D, new PlayerMoveRightCommand(_player));
        keyboardController.RegisterCommand(Keys.A, new PlayerMoveLeftCommand(_player));
        keyboardController.RegisterCommand(Keys.Space, new PlayerJumpCommand(_player));
        keyboardController.RegisterCommand(Keys.N, new PlayerAttackCommand(_player));
        keyboardController.RegisterCommand(Keys.Z, new PlayerAttackCommand(_player));
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
        keyboardController.RegisterPressCommand(Keys.O, new PreviousEnemyCommand(_enemyCycler));
        keyboardController.RegisterPressCommand(Keys.P, new NextEnemyCommand(_enemyCycler));

        mouseController.RegisterCommand(MouseButton.LeftButton, new SetRockingPlayerSpriteCommand(_player));

        _controllers = new List<IController> { keyboardController, mouseController };
    }

    protected override void Update(GameTime gameTime)
    {
        _player.Update(gameTime);
        _ground.Update(gameTime);
        _tiles.Update(gameTime);
        _items.Update(gameTime);
        foreach (var projectile in _projectiles) projectile.Update(gameTime);
        _projectiles.RemoveAll(projectile => projectile.IsFinished);
        //foreach (var enemy in _enemies) enemy.Update(gameTime);    DONT REMOVE
        //_enemies.RemoveAll(enemy => enemy.IsDead);     DONT REMOVE
        _enemyCycler.Update(gameTime);

        foreach (var controller in _controllers) controller.Update();
        _player.HandleKeeseCollision(_keeseEnemy);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);

        _spriteBatch.Begin();
        _background.Draw(_spriteBatch);
        _ground.Draw(_spriteBatch);
        _tiles.Draw(_spriteBatch);
        _items.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);
        foreach (var projectile in _projectiles) projectile.Draw(_spriteBatch);
        //foreach (var enemy in _enemies) enemy.Update(gameTime);  DONT REMOVE
        _enemyCycler.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
