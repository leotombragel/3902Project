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
    private SoundEffect _blockBreaking;
    private SoundEffect _bowShot;
    private SoundEffect _collectRupee;
    private SoundEffect _enemyDeath;
    private SoundEffect _explosion;
    private SoundEffect _fireball;
    private SoundEffect _jump;
    private SoundEffect _openBlock;
    private SoundEffect _pipeTravel;
    private SoundEffect _playerDeath;
    private SoundEffect _playerHit;

    private SoundEffect _swordAttack;
    private SoundEffect _triforce;


    public void LoadAllContent(ContentManager content)
    {
        _explosion = content.Load<SoundEffect>("sound effects/explosion");
        _blockBreaking = content.Load<SoundEffect>("sound effects/blockBreaking");
        _bowShot = content.Load<SoundEffect>("sound effects/bowShot");
        _collectRupee = content.Load<SoundEffect>("sound effects/collectRupee");
        _enemyDeath = content.Load<SoundEffect>("sound effects/enemyDeath");
        _fireball = content.Load<SoundEffect>("sound effects/fireball");
        _jump = content.Load<SoundEffect>("sound effects/jump");
        _openBlock = content.Load<SoundEffect>("sound effects/openBlock");
        _pipeTravel = content.Load<SoundEffect>("sound effects/pipeTravel");
        _playerDeath = content.Load<SoundEffect>("sound effects/playerDeath");
        _playerHit = content.Load<SoundEffect>("sound effects/playerHit");
        _swordAttack = content.Load<SoundEffect>("sound effects/swordAttack");
        _triforce = content.Load<SoundEffect>("sound effects/triforce");
    }


    public void PlayBlockBreakingSFX()
    {
        _blockBreaking.Play(1.0f, 0.0f, 0.0f);
    }

    public void PlayBowShotSFX()
    {
        _bowShot.Play(0.5f, 0.0f, 0.0f);
    }

    public void PlayCollectRupeeSFX()
    {
        _collectRupee.Play(0.5f, 0.0f, 0.0f);
    }

    public void PlayEnemyDeathSFX()
    {
        _enemyDeath.Play(0.5f, 0.0f, 0.0f);
    }

    public void PlayExplosionSFX()
    {
        _explosion.Play(0.5f, 0.0f, 0.0f);
    }

    public void PlayFireballSFX()
    {
        _fireball.Play(0.5f, 0.0f, 0.0f);
    }

    public void PlayJumpSFX()
    {
        _jump.Play(0.5f, 0.0f, 0.0f);
    }

    public void PlayOpenBlockSFX()
    {
        _openBlock.Play(0.5f, 0.0f, 0.0f);
    }

    public void PlayPipeTravelSFX()
    {
        _pipeTravel.Play(0.5f, 0.0f, 0.0f);
    }

    public void PlayPlayerDeathSFX()
    {
        _playerDeath.Play(0.5f, 0.0f, 0.0f);
    }

    public void PlayPlayerHitSFX()
    {
        _playerHit.Play(0.3f, 0.0f, 0.0f);
    }

    public void PlaySwordAttackSFX()
    {
        _swordAttack.Play(0.5f, 0.0f, 0.0f);
    }

    public void PlayTriforceSFX()
    {
        _triforce.Play(0.1f, 0.0f, 0.0f);
    }
}
