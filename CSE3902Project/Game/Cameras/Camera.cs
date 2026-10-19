using System;
using Microsoft.Xna.Framework;

namespace CSE3902Project.Game.Cameras;

/// camera that keeps track of which part of the stage is on screen
public class Camera
{
    //top left corner of the screen, in stage coordinates
    public Vector2 Position { get; set; }
    public int ViewWidth { get; }
    public int ViewHeight { get; }

    public Camera(int viewWidth, int viewHeight)
    {
        ViewWidth = viewWidth;
        ViewHeight = viewHeight;
    }

    //draw the stage with this so everything slides the opposite way the camera moves
    public Matrix GetTransform()
    {
        //round so the pixel art doesn't get blurry
        float x = (float)Math.Round(Position.X);
        float y = (float)Math.Round(Position.Y);
        return Matrix.CreateTranslation(-x, -y, 0);
    }
}
