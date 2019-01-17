using System;
using System.Collections.Generic;
using System.Linq;
using RenderEngine;

namespace Chess3D
{
    static class Program
    {
        static void Main()
        {
            new DisplayManager().Run(60);
        }
    }
}
