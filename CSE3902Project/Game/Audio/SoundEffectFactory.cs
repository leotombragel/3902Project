using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE3902Project.Game.Audio;

public class SoundEffectFactory
{
    public static SoundEffectFactory Instance { get; } = new SoundEffectFactory();
    private SoundEffect _explosion;

    public void LoadAllContent(ContentManager content)
    {
        _explosion = content.Load<SoundEffect>("sound effects/explosion");
    }

    public void PlayExplosionSFX()
    {
        _explosion.Play();
    }
}
