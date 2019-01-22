using System;
using RenderEngine;

namespace World3D
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                new DisplayManager().Run(120);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something bad happened. Application must be closed.");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
