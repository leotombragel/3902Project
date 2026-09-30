using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CSE3902Project.Game.Command;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using CSE3902Project.Game.Input;
using CSE3902Project.Game.Tiles;

namespace CSE3902Project;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private List<IController> _controllers;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Player _player;
    private TileCycler _tiles;
    private GroundRow _ground;
    private KeeseEnemy _keeseEnemy;
    private StalfoEnemy _stalfoEnemy;
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

        var keyboardController = new KeyboardController();
        var mouseController = new MouseController();

        _player = new Player();

        var viewport = GraphicsDevice.Viewport;
        _ground = new GroundRow(viewport.Width, viewport.Height);
        _tiles = new TileCycler(new Vector2((viewport.Width - Tile.Size) / 2f, (viewport.Height - Tile.Size) / 2f));
        _keeseEnemy = new KeeseEnemy();
        _stalfoEnemy = new StalfoEnemy();
        _wizzrobeEnemy = new WizzrobeEnemy();

        // Bind commands to button presses
        keyboardController.RegisterCommand(Keys.D, new PlayerMoveRightCommand(_player));
        keyboardController.RegisterCommand(Keys.A, new PlayerMoveLeftCommand(_player));
        keyboardController.RegisterCommand(Keys.Space, new PlayerJumpCommand(_player));
        keyboardController.RegisterCommand(Keys.N, new PlayerAttackCommand(_player));
        keyboardController.RegisterCommand(Keys.Z, new PlayerAttackCommand(_player));
        keyboardController.RegisterCommand(Keys.Escape, new ExitCommand(this));
        keyboardController.RegisterPressCommand(Keys.T, new PreviousTileCommand(_tiles));
        keyboardController.RegisterPressCommand(Keys.Y, new NextTileCommand(_tiles));

        mouseController.RegisterCommand(MouseButton.LeftButton, new SetRockingPlayerSpriteCommand(_player));

        _controllers = new List<IController> { keyboardController, mouseController };
    }

    protected override void Update(GameTime gameTime)
    {
        _player.Update(gameTime);
        _ground.Update(gameTime);
        _tiles.Update(gameTime);
        _keeseEnemy.Update(gameTime);
        _stalfoEnemy.Update(gameTime);
        _wizzrobeEnemy.setPlayerPos(_player.Position); //give link's position to wizzrobe
        _wizzrobeEnemy.Update(gameTime);

        foreach (var controller in _controllers) controller.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);

        _spriteBatch.Begin();
        _ground.Draw(_spriteBatch);
        _tiles.Draw(_spriteBatch);
        _player.Draw(_spriteBatch);
        _keeseEnemy.Draw(_spriteBatch);
        _stalfoEnemy.Draw(_spriteBatch);
        _wizzrobeEnemy.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
