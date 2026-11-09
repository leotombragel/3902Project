using CSE3902Project.Game.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MusicFactory
{
    public static MusicFactory Instance { get; } = new MusicFactory();


    public Song _overworld;
    public void LoadAllMusic(ContentManager content)
    {
        _overworld = content.Load<Song>("music/zOverworld");      
    }

    public void PlayOverworldMusic()
    {
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.5f;
        MediaPlayer.Play(_overworld);
    }

    public void toggleMusic()
    {
        if(MediaPlayer.Volume == 0.0f)
        {
            MediaPlayer.Volume = 0.5f;
        }
        else
        {
            MediaPlayer.Volume = 0.0f;
        }

         
    }
}
