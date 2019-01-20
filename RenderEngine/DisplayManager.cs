using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private Stopwatch stopwatch = new Stopwatch();
        private KeyboardHelper keyboard = new KeyboardHelper();
        private MouseHelper mouse = new MouseHelper();

        private Loader loader = new Loader();
        private BaseCamera camera;

        private List<Entity> entities;
        private List<Terrain> terrains;
        private List<Light> lights;
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

            TexturedModel tree = loader.CreateTexturedModel("tree", "tree.png");
            TexturedModel fernTextureAtlas = loader.CreateTexturedModel("fern", "fern.png", 2);
            TexturedModel playerModel = loader.CreateTexturedModel("player", "playerTexture.png");

            fernTextureAtlas.Texture.HasTransparency = true;
            fernTextureAtlas.Texture.UseFakeLightning = true;

            tree.Texture.ShineDampler = 10;
            tree.Texture.Reflectivity = 1;

            terrains = new List<Terrain>()
            {
                new Terrain(0, -1, loader, texturePack, blendMap, "heightMap.png"),
                new Terrain(-1, -1, loader, texturePack, blendMap, "heightMap.png")
            };

            Random rdn = new Random();
            entities = new List<Entity>();

            for (int i = 0; i < 1200; i++)
            {
                float x = (float)rdn.NextDouble() * 800 - 400;
                float z = (float)rdn.NextDouble() * -600;
                float y = terrains.Where(t => t.IsOnTerrain(new Vector3(x, 0, z))).FirstOrDefault()?.GetHeight(x, z) ?? 0;

                if (i % 2 == 0)
                {
                    entities.Add(new Entity(tree, new Vector3(x, y, z), new Vector3(0, 0, 0), 3));
                }
                else
                {
                    entities.Add(new Entity(fernTextureAtlas, rdn.Next() % 4, new Vector3(x, y, z), new Vector3(0, 0, 0), 0.6f));
                }
            }

            lights = new List<Light>()
            {
                new Light(new Vector3(0, 2000, 2000), new Vector3(1, 1, 1)),
                //new Light(new Vector3(-200, 10, -200), new Vector3(10,0,0)),
                //new Light(new Vector3(200,10,200), new Vector3(0,0,10))
            };

            player = new Player(playerModel, new Vector3(0, 0, -50), new Vector3(0, 180, 0), 0.5f);

            camera = new Camera(keyboard, mouse);
            camera = new ThirdPersonCamera(keyboard, mouse, player);

            stopwatch.Start();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            long delta = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            var terrainWherePlayerStands = terrains.Where(t => t.IsOnTerrain(player.Position)).FirstOrDefault();
            player.Move(keyboard, (float x, float y) => terrainWherePlayerStands?.GetHeight(x, y) ?? 0, delta);
            camera.Move();

            mouse.ResetDeltas();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            foreach (var terrain in terrains)
            {
                renderer.ProcessTerrain(terrain);
            }

            renderer.ProcessEntity(player);

            foreach (var entity in entities)
            {
                renderer.ProcessEntity(entity);
            }

            renderer.Render(lights, camera);

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
