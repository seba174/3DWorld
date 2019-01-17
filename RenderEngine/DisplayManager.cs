using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using OpenTK.Graphics;
using Shaders;

namespace RenderEngine
{
    public class DisplayManager : GameWindow
    {
        private Loader loader;
        private Renderer renderer;
        RawModel model;
        StaticShader shader;

        float[] vertices =
        {
            -0.5f, 0.5f, 0f,
            -0.5f, -0.5f, 0f,
            0.5f, -0.5f, 0f,
            0.5f, -0.5f, 0f,
            0.5f, 0.5f, 0f,
            -0.5f, 0.5f, 0f
        };

        public DisplayManager()
            : base(1280, 720, GraphicsMode.Default, "Chess3D", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            float[] vertices =
            {
                -0.5f, 0.5f, 0f,
                -0.5f, -0.5f, 0f,
                0.5f, -0.5f, 0f,
                0.5f, 0.5f, 0f,
            };

            int[] indices =
            {
                0,1,3,
                3,1,2
            };

            loader = new Loader();
            renderer = new Renderer();
            model = loader.LoadToVAO(vertices, indices);
            shader = new StaticShader();
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            renderer.Prepare();

            shader.Start();
            renderer.Render(model);
            shader.Stop();

            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            loader.CleanUp();
            shader.CleanUp();

            base.OnUnload(e);
        }
    }
}
