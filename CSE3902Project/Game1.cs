using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CSE3902Project.Game.Command;
using CSE3902Project.Game.Entity;
using CSE3902Project.Game.Graphics;
using CSE3902Project.Game.Input;

namespace CSE3902Project;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Player _player;
    private KeeseEnemy _keeseEnemy;
    private StalfoEnemy _stalfoEnemy;

    private List<IController> _controllers;

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
        TextCreator.Initialize(Content);

        var keyboardController = new KeyboardController();
        var mouseController = new MouseController();
        
        _player = new Player();
        _keeseEnemy = new KeeseEnemy();
        _stalfoEnemy = new StalfoEnemy();
        
        // Bind commands to button presses
        keyboardController.RegisterCommand(Keys.D, new PlayerMoveRightCommand(_player));
        keyboardController.RegisterCommand(Keys.A, new PlayerMoveLeftCommand(_player));
        keyboardController.RegisterCommand(Keys.Space, new PlayerJumpCommand(_player));
        keyboardController.RegisterCommand(Keys.Escape, new ExitCommand(this));
        
        mouseController.RegisterCommand(MouseButton.LeftButton, new SetRockingPlayerSpriteCommand(_player));
        
        _controllers = new List<IController> { keyboardController, mouseController };
    }

    protected override void Update(GameTime gameTime)
    {
        _player.Update(gameTime);
        _keeseEnemy.Update(gameTime);
        _stalfoEnemy.Update(gameTime);
        
        foreach (var controller in _controllers)
        {
            controller.Update();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();
        TextCreator.CreateSprint0Text(Window, _spriteBatch);
        _player.Draw(_spriteBatch);
        _keeseEnemy.Draw(_spriteBatch);
        _stalfoEnemy.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}