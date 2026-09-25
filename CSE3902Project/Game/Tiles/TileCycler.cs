using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Tiles;

/// Shows one tile at a time and lets the player step through every tile type (Sprint 2 "t"/"y" keys).
public class TileCycler
{
    private static readonly TileType[] AllTypes = Enum.GetValues<TileType>();

    private readonly Vector2 _position;
    private int _index;
    private ITile _currentTile;

    public TileCycler(Vector2 position)
    {
        _position = position;
        Reset();
    }

    public void Next() => Select(_index + 1);

    public void Previous() => Select(_index - 1);

    public void Reset() => Select(0);

    public void Update(GameTime gameTime)
    {
        _currentTile.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _currentTile.Draw(spriteBatch);
    }

    private void Select(int index)
    {
        _index = (index % AllTypes.Length + AllTypes.Length) % AllTypes.Length;
        _currentTile = TileFactory.Create(AllTypes[_index], _position);
    }
}
