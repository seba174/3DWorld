using System;
using System.Collections.Generic;
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
        private TexturedModel staticModel, grass, fern;
        private List<Entity> entities;
        private Camera camera;
        private Light light;
        private Terrain terrain, terrain2;
        private MasterRenderer renderer;

        private bool keyW, keyD, keyA;

        public DisplayManager()
            : base(1280, 720, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 0, 4), "Chess3D", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {
            GL.Enable(EnableCap.Multisample);
        }

        protected override void OnLoad(EventArgs e)
        {
            loader = new Loader();

            staticModel = new TexturedModel(OBJLoader.LoadObjModel("tree", loader), new ModelTexture(loader.InitTexture("tree.png")));
            grass = new TexturedModel(OBJLoader.LoadObjModel("grassModel", loader), new ModelTexture(loader.InitTexture("grassTexture.png")));
            fern = new TexturedModel(OBJLoader.LoadObjModel("fern", loader), new ModelTexture(loader.InitTexture("fern.png")));

            grass.Texture.HasTransparency = true;
            fern.Texture.HasTransparency = true;

            grass.Texture.UseFakeLightning = true;
            fern.Texture.UseFakeLightning = true;

            staticModel.Texture.ShineDampler = 10;
            staticModel.Texture.Reflectivity = 1;

            Random rdn = new Random();
            entities = new List<Entity>();

            for (int i = 0; i < 500; i++)
            {
                entities.Add(new Entity(staticModel, new Vector3((float)rdn.NextDouble() * 800 - 400, 0, (float)rdn.NextDouble() * -600),
                    new Vector3(0, 0, 0), 3));
                entities.Add(new Entity(grass, new Vector3((float)rdn.NextDouble() * 800 - 400, 0, (float)rdn.NextDouble() * -600),
                    new Vector3(0, 0, 0), 1));
                entities.Add(new Entity(fern, new Vector3((float)rdn.NextDouble() * 800 - 400, 0, (float)rdn.NextDouble() * -600),
                    new Vector3(0, 0, 0), 0.6f));
            }

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
            camera.Move(keyW, keyD, keyA);

            renderer.ProcessTerrain(terrain);
            renderer.ProcessTerrain(terrain2);

            foreach (var entity in entities)
            {
                renderer.ProcessEntity(entity);
            }

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
