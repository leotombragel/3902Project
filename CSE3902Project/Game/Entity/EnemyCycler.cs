using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CSE3902Project.Game.Entity;

public class EnemyCycler
{
    private readonly List<IEnemy> _enemies;
    private int _index;

    public EnemyCycler(List<IEnemy> enemies)
    {
        _enemies = enemies;
    }

    public void Next()
    {
        if (_enemies.Count == 0) return;
        _index = (_index + 1) % _enemies.Count;
    }

    public void Previous()
    {
        if (_enemies.Count == 0) return;
        _index = (_index - 1 + _enemies.Count) % _enemies.Count;
    }

    public void Update(GameTime gameTime)
    {
        if (_enemies.Count == 0) return;
        _enemies[_index].Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_enemies.Count == 0) return;
        _enemies[_index].Draw(spriteBatch);
    }
}