using System;
using System.Collections.Generic;
using System.Diagnostics;
using Entities;
using InputHandlings;
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
        private Loader loader = new Loader();
        private List<Entity> entities;
        private Camera camera = new Camera();
        private Light light;
        private Terrain terrain, terrain2;
        private MasterRenderer renderer;
        private KeyboardHelper keyboard = new KeyboardHelper();
        private Player player;

        public DisplayManager()
            : base(1280, 720, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 0, 4), "Chess3D", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
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

            //TexturedModel chessBoard = new TexturedModel(loader.LoadToVAO("boardWork"), new ModelTexture(loader.InitTexture("TableroDiffuse01.png")));

            TexturedModel tree = new TexturedModel(loader.LoadToVAO("tree"), new ModelTexture(loader.InitTexture("tree.png")));
            TexturedModel grass = new TexturedModel(loader.LoadToVAO("grassModel"), new ModelTexture(loader.InitTexture("grassTexture.png")));
            TexturedModel fern = new TexturedModel(loader.LoadToVAO("fern"), new ModelTexture(loader.InitTexture("fern.png")));
            TexturedModel playerModel = new TexturedModel(loader.LoadToVAO("player"), new ModelTexture(loader.InitTexture("playerTexture.png")));

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

            light = new Light(new Vector3(2000, 2000, 2000), new Vector3(1, 1, 1));
            
            terrain = new Terrain(0, 0, loader, texturePack, blendMap);
            terrain2 = new Terrain(1, 0, loader, texturePack, blendMap);

            player = new Player(playerModel, new Vector3(0, 0, -50), new Vector3(0, 0, 0), 0.5f);

            stopwatch.Start();
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

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            long delta = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            camera.Move(keyboard);
            player.Move(keyboard, delta);
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
    }
}
