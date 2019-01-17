using System;
using Entities;
using Models;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
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
        private TexturedModel staticModel;
        private Entity entity;
        private Camera camera;

        private bool keyW, keyD, keyA;

        public DisplayManager()
            : base(1280, 720, GraphicsMode.Default, "Chess3D", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            float[] vertices = {
                -0.5f,0.5f,0,
                -0.5f,-0.5f,0,
                0.5f,-0.5f,0,
                0.5f,0.5f,0,

                -0.5f,0.5f,1,
                -0.5f,-0.5f,1,
                0.5f,-0.5f,1,
                0.5f,0.5f,1,

                0.5f,0.5f,0,
                0.5f,-0.5f,0,
                0.5f,-0.5f,1,
                0.5f,0.5f,1,

                -0.5f,0.5f,0,
                -0.5f,-0.5f,0,
                -0.5f,-0.5f,1,
                -0.5f,0.5f,1,

                -0.5f,0.5f,1,
                -0.5f,0.5f,0,
                0.5f,0.5f,0,
                0.5f,0.5f,1,

                -0.5f,-0.5f,1,
                -0.5f,-0.5f,0,
                0.5f,-0.5f,0,
                0.5f,-0.5f,1

        };

            float[] textureCoords = {

                0,0,
                0,1,
                1,1,
                1,0,
                0,0,
                0,1,
                1,1,
                1,0,
                0,0,
                0,1,
                1,1,
                1,0,
                0,0,
                0,1,
                1,1,
                1,0,
                0,0,
                0,1,
                1,1,
                1,0,
                0,0,
                0,1,
                1,1,
                1,0


        };

            int[] indices = {
                0,1,3,
                3,1,2,
                4,5,7,
                7,5,6,
                8,9,11,
                11,9,10,
                12,13,15,
                15,13,14,
                16,17,19,
                19,17,18,
                20,21,23,
                23,21,22

        };

            loader = new Loader();
            model = loader.LoadToVAO(vertices, textureCoords, indices);
            shader = new StaticShader();
            renderer = new Renderer(Width, Height, shader);
            
            staticModel = new TexturedModel(model, new ModelTexture(loader.InitTexture("image.png")));
            entity = new Entity(staticModel, new Vector3(0, 0, -5), new Vector3(0, 0, 0), 1);
            camera = new Camera();
        }


        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            ProcessKeyInput(e, true);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            ProcessKeyInput(e, false);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            entity.Rotation += new Vector3(1, 1, 0);
            camera.Move(keyW, keyD, keyA);
            renderer.Prepare();

            shader.Start();
            shader.LoadViewMatrix(camera);
            renderer.Render(entity, shader);
            shader.Stop();

            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            loader.CleanUp();
            shader.CleanUp();

            base.OnUnload(e);
        }

        private void ProcessKeyInput(KeyboardKeyEventArgs e, bool enable)
        {
            switch (e.Key)
            {
                case Key.A:
                    keyA = enable; break;
                case Key.D:
                    keyD = enable; break;
                case Key.W:
                    keyW = enable; break;
            }
        }
    }
}
