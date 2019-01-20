using System;
using System.Collections.Generic;
using System.Diagnostics;
using Entities;
using InputHandling;
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
        Stopwatch stopwatch = new Stopwatch();
        private KeyboardHelper keyboard = new KeyboardHelper();
        private MouseHelper mouse = new MouseHelper();

        private Loader loader = new Loader();
        private BaseCamera camera;

        private List<Entity> entities;
        private Light light;
        private Terrain terrain, terrain2;
        private MasterRenderer renderer;
        private Player player;

        public DisplayManager()
            : base(1280, 720, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 0, 4), "Chess3D",
                  GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {
            GL.Enable(EnableCap.Multisample);
        }

        protected override void OnLoad(EventArgs e)
        {
            renderer = new MasterRenderer(Height, Width);

            TerrainTexture backgroundTexture = new TerrainTexture(loader.InitTexture("grassy.png"));
            TerrainTexture rTexture = new TerrainTexture(loader.InitTexture("mud.png"));
            TerrainTexture gTexture = new TerrainTexture(loader.InitTexture("grassFlowers.png"));
            TerrainTexture bTexture = new TerrainTexture(loader.InitTexture("path.png"));
            TerrainTexture blendMap = new TerrainTexture(loader.InitTexture("blendMap.png"));
            TerrainTexturePack texturePack = new TerrainTexturePack(backgroundTexture, rTexture, gTexture, bTexture);

            //TexturedModel chessBoard = loader.CreateTexturedModel("boardWork", "TableroDiffuse01.png");

            TexturedModel tree = loader.CreateTexturedModel("tree","tree.png");
            TexturedModel grass = loader.CreateTexturedModel("grassModel", "grassTexture.png");
            TexturedModel fern = loader.CreateTexturedModel("fern", "fern.png");
            TexturedModel playerModel = loader.CreateTexturedModel("player", "playerTexture.png");

            grass.Texture.HasTransparency = true;
            fern.Texture.HasTransparency = true;

            grass.Texture.UseFakeLightning = true;
            fern.Texture.UseFakeLightning = true;

            tree.Texture.ShineDampler = 10;
            tree.Texture.Reflectivity = 1;

            Random rdn = new Random();
            entities = new List<Entity>();

            for (int i = 0; i < 500; i++)
            {
                //entities.Add(new Entity(chessBoard, new Vector3((float)rdn.NextDouble() * 800 - 400, 5, (float)rdn.NextDouble() * -600),
                //    new Vector3(180, 0, 0), 3));
                entities.Add(new Entity(tree, new Vector3((float)rdn.NextDouble() * 800 - 400, 0, (float)rdn.NextDouble() * -600),
                    new Vector3(0, 0, 0), 3));
                entities.Add(new Entity(grass, new Vector3((float)rdn.NextDouble() * 800 - 400, 0, (float)rdn.NextDouble() * -600),
                    new Vector3(0, 0, 0), 1));
                entities.Add(new Entity(fern, new Vector3((float)rdn.NextDouble() * 800 - 400, 0, (float)rdn.NextDouble() * -600),
                    new Vector3(0, 0, 0), 0.6f));
            }

            camera = new Camera(keyboard, mouse);
            light = new Light(new Vector3(2000, 2000, 2000), new Vector3(1, 1, 1));
            
            terrain = new Terrain(0, 0, loader, texturePack, blendMap, "heightMap.png");
            terrain2 = new Terrain(1, 0, loader, texturePack, blendMap, "heightMap.png");

            player = new Player(playerModel, new Vector3(0, 0, -50), new Vector3(0, 0, 0), 0.5f);

            camera = new ThirdPersonCamera(keyboard, mouse, player);

            stopwatch.Start();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            long delta = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            player.Move(keyboard, delta);
            camera.Move();

            mouse.ResetDeltas();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            renderer.ProcessEntity(player);
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


        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            keyboard.Update(e, true);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            keyboard.Update(e, false);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            mouse.UpdateMouseWheel(e);
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouse.UpdateMouseMove(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            mouse.UpdateMouseButtons(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            mouse.UpdateMouseButtons(e);
        }
    }
}
