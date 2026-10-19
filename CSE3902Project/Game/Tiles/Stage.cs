using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Tiles;

/// the level the player walks around in. for now it is just one long flat row of ground
public class Stage
{
    //how wide the stage is in tiles (a bit over 3 screens so the camera has room to move)
    private const int WidthInTiles = 80;

    private readonly GroundRow _ground;

    //y position of the top of the ground
    public int GroundY { get; }

    //the whole stage (wider than the screen)
    public Rectangle Bounds { get; }

    //everything the player can stand on or run into
    public List<ITile> SolidTiles { get; }

    public Stage(int screenHeight)
    {
        int stageWidth = WidthInTiles * Tile.Size;

        _ground = new GroundRow(stageWidth, screenHeight);
        GroundY = _ground.TileHeight;
        Bounds = new Rectangle(0, 0, stageWidth, screenHeight);
        SolidTiles = new List<ITile>(_ground.Tiles);
    }

    public void Update(GameTime gameTime)
    {
        _ground.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _ground.Draw(spriteBatch);
    }
}
