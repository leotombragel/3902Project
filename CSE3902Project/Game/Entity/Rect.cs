using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSE3902Project.Game.Entity;

public class Rect
{
    public float x1;
    public float y1;
    public float x2;
    public float y2;
    public Rect(float a, float b, float c, float d)
    {
        x1 = a;
        y1 = b;
        x2 = c;
        y2 = d;
    }
}
