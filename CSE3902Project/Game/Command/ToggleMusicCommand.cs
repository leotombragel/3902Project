using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE3902Project.Game.Command;


public class ToggleMusicCommand : ICommand
{
    private readonly MusicFactory _music;

    public ToggleMusicCommand(MusicFactory music)
    {
        _music = music;
    }

    public void Execute()
    {
        _music.ToggleMusic();
    }
}