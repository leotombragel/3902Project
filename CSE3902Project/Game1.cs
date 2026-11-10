using CSE3902Project.Game.Graphics;
using CSE3902Project.Game.State;
using Gum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private StateManager _stateManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        //_graphics.PreferredBackBufferWidth = 1000;   change window size here
        //_graphics.PreferredBackBufferHeight = 750;
    }

    protected override void Initialize()
    {
        GumService.Default.Initialize(this);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        SpriteFactory.Instance.LoadAllAssets(Content);
        TileSpriteFactory.Instance.LoadAllAssets(Content);
        ItemSpriteFactory.Instance.LoadAllAssets(Content);
        BackgroundSpriteFactory.Instance.LoadAllAssets(Content);
        HudSpriteFactory.Instance.LoadAllAssets(Content, GraphicsDevice);
        TextCreator.Initialize(Content);
        MusicFactory.Instance.LoadAllMusic(Content);

        _stateManager = new StateManager();
        _stateManager.Push(new GameplayState(GraphicsDevice, _stateManager, this));
    }

    protected override void Update(GameTime gameTime)
    {
        _stateManager.Update(gameTime);
        GumService.Default.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);

        _stateManager.Draw(gameTime, _spriteBatch);
        GumService.Default.Draw();

        base.Draw(gameTime);
    }
}
