using System;
using Entities;
using Models;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Terrains;
using Textures;
using ToolBox;

namespace RenderEngine
{
    public class DisplayManager : GameWindow
    {
        private Loader loader;
        private RawModel model;
        private TexturedModel staticModel;
        private Entity entity;
        private Camera camera;
        private Light light;
        private Terrain terrain, terrain2;
        private MasterRenderer renderer;

        private bool keyW, keyD, keyA;

        public DisplayManager()
            : base(1280, 720, GraphicsMode.Default, "Chess3D", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            loader = new Loader();
            model = OBJLoader.LoadObjModel("tree.obj", loader);

            staticModel = new TexturedModel(model, new ModelTexture(loader.InitTexture("tree.png")));
            var texture = staticModel.Texture;
            texture.ShineDampler = 10;
            texture.Reflectivity = 1;

            entity = new Entity(staticModel, new Vector3(0, 0, -25), new Vector3(0, 0, 0), 1);
            light = new Light(new Vector3(2000, 2000, 2000), new Vector3(1, 1, 1));

            terrain = new Terrain(0, 0, loader, new ModelTexture(loader.InitTexture("grass.png")));
            terrain2 = new Terrain(1, 0, loader, new ModelTexture(loader.InitTexture("grass.png")));


            renderer = new MasterRenderer(Height, Width);

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
            entity.Rotation += new Vector3(0, 1, 0);
            camera.Move(keyW, keyD, keyA);

            renderer.ProcessTerrain(terrain);
            renderer.ProcessTerrain(terrain2);
            renderer.ProcessEntity(entity);

            renderer.Render(light, camera);

            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            renderer.CleanUp();
            loader.CleanUp();

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
