using OpenTK.Graphics.OpenGL4;
using RenderEngine;

namespace Chess3D
{
    internal static class Program
    {
        private static void Main()
        {
            new DisplayManager().Run(120);
        }
    }
}
