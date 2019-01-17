using System;
using Models;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Shaders;
using Textures;

namespace RenderEngine
{
    public class DisplayManager : GameWindow
    {
        private Loader loader;
        private Renderer renderer;
        private RawModel model;
        private StaticShader shader;
        private ModelTexture texture;
        private TexturedModel texturedModel;

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

            float[] textureCoords =
            {
                0,0,
                0,1,
                1,1,
                1,0
            };

            loader = new Loader();
            renderer = new Renderer();
            model = loader.LoadToVAO(vertices, textureCoords, indices);
            shader = new StaticShader();
            texture = new ModelTexture(loader.InitTexture("image.png"));
            texturedModel = new TexturedModel(model, texture);
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
            renderer.Render(texturedModel);
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
