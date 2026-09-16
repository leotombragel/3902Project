using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint0_2.Game.Command;
using Sprint0_2.Game.Entity;
using Sprint0_2.Game.Graphics;
using Sprint0_2.Game.Input;

namespace Sprint0_2;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Player _player;

    private List<IController> _controllers;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        var keyboardController = new KeyboardController();
        var mouseController = new MouseController();
        
        _player = new Player();
        
        // Bind commands to button presses
        keyboardController.RegisterCommand(Keys.D, new PlayerMoveRightCommand(_player));
        keyboardController.RegisterCommand(Keys.A, new PlayerMoveLeftCommand(_player));
        keyboardController.RegisterCommand(Keys.Space, new PlayerJumpCommand(_player));
        keyboardController.RegisterCommand(Keys.Escape, new ExitCommand(this));
        
        mouseController.RegisterCommand(MouseButton.LeftButton, new SetRockingPlayerSpriteCommand(_player));
        
        _controllers = new List<IController> { keyboardController, mouseController };
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice); // Texture rendering
        
        SpriteFactory.Instance.LoadAllAssets(Content); // Player sprites
        TextCreator.Initialize(Content);

        _player.Sprite = SpriteFactory.Instance.CreateIdlePlayerSprite();
    }

    protected override void Update(GameTime gameTime)
    {
        _player.Update(gameTime);
        
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
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}