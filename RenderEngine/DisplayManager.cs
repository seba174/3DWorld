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
        private ScreenHelper screen = new ScreenHelper();
        private MousePicker mousePicker;
        private DayTime dayTime = new DayTime();

        private Loader loader = new Loader();
        private BaseCamera camera;

        private List<Entity> entities;
        private List<Terrain> terrains;
        private List<Light> lights;
        private MasterRenderer renderer;
        private Player player;

        private Light Sun => lights.First();
        private IEnumerable<Light> Lamps => lights.Skip(1);


        public DisplayManager()
            : base(1600, 1000, new GraphicsMode(new ColorFormat(8, 8, 8, 8), 24, 0, 4), "Chess3D",
                  GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {
            screen = new ScreenHelper()
            {
                Width = Width,
                Height = Height
            };
            GL.Enable(EnableCap.Multisample);
        }

        protected override void OnLoad(EventArgs e)
        {
            renderer = new MasterRenderer(screen, loader);

            TerrainTexture backgroundTexture = new TerrainTexture(loader.InitTexture("grassy"));
            TerrainTexture rTexture = new TerrainTexture(loader.InitTexture("mud"));
            TerrainTexture gTexture = new TerrainTexture(loader.InitTexture("grassFlowers"));
            TerrainTexture bTexture = new TerrainTexture(loader.InitTexture("path"));
            TerrainTexture blendMap = new TerrainTexture(loader.InitTexture("blendMap"));
            TerrainTexturePack texturePack = new TerrainTexturePack(backgroundTexture, rTexture, gTexture, bTexture);

            TexturedModel tree = loader.CreateTexturedModel("tree", "tree");
            TexturedModel fernTextureAtlas = loader.CreateTexturedModel("fern", "fern", 2);
            TexturedModel playerModel = loader.CreateTexturedModel("player", "playerTexture");
            TexturedModel lamp = loader.CreateTexturedModel("lamp", "lamp");

            fernTextureAtlas.Texture.HasTransparency = true;
            fernTextureAtlas.Texture.UseFakeLightning = true;

            tree.Texture.ShineDampler = 10;
            tree.Texture.Reflectivity = 1;

            terrains = new List<Terrain>()
            {
                new Terrain(0, -1, loader, texturePack, blendMap, "heightMap"),
                new Terrain(-1, -1, loader, texturePack, blendMap, "heightMap")
            };

            Random rdn = new Random();
            entities = new List<Entity>();

            lights = new List<Light>()
            {
                new Light(new Vector3(0, 1000, 2000), new Vector3(0.2f, 0.2f, 0.2f)),
            };

            for (int i = 0; i < 1000; i++)
            {
                float x = (float)rdn.NextDouble() * 800 - 400;
                float z = (float)rdn.NextDouble() * -600;
                float y = terrains.Where(t => t.IsOnTerrain(new Vector3(x, 0, z))).FirstOrDefault()?.GetHeight(x, z) ?? 0;

                if (i % 100 == 0)
                {
                    CreateLamp(new Vector3(x, y, z), new Vector3(3.5f, 2, 2), lamp);
                }
                else if (i % 2 == 0)
                {
                    entities.Add(new Entity(tree, new Vector3(x, y, z), new Vector3(0, 0, 0), 3));
                }
                else
                {
                    entities.Add(new Entity(fernTextureAtlas, rdn.Next() % 4, new Vector3(x, y, z), new Vector3(0, 0, 0), 0.6f));
                }
            }

            player = new Player(playerModel, new Vector3(0, 0, -50), new Vector3(0, 180, 0), 0.5f);

            camera = new Camera(keyboard, mouse);
            camera = new ThirdPersonCamera(keyboard, mouse, player);

            mousePicker = new MousePicker(renderer.ProjectionMatrix, mouse, screen);

            stopwatch.Start();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (stopwatch.ElapsedMilliseconds > 0)
            {
                dayTime.Update(stopwatch.ElapsedMilliseconds);
                UpdateLightsBasedOnDayTime();
            }
            stopwatch.Restart();

            var terrainWherePlayerStands = terrains.Where(t => t.IsOnTerrain(player.Position)).FirstOrDefault();
            player.Move(keyboard, (float x, float y) => terrainWherePlayerStands?.GetHeight(x, y) ?? 0, dayTime.LastFrameTime);
            camera.Move();
            //mousePicker.Update(camera);
            //Console.WriteLine(mousePicker.CurrentRay);

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

            renderer.Render(lights, camera, dayTime);

            SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            renderer.CleanUp();
            loader.CleanUp();

            base.OnUnload(e);
        }

        #region Events

        protected override void OnResize(EventArgs e)
        {
            screen.Height = Height;
            screen.Width = Width;

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

        #endregion

        private void CreateLamp(Vector3 position, Vector3 lightCoulour, TexturedModel lampModel)
        {
            float scale = 1.0f;
            float lightSourceHeightFactor = 0.97f;
            Vector3 lampAttenuation = new Vector3(1, 0.01f, 0.002f);

            lampModel.Texture.UseFakeLightning = true;

            Entity lamp = new Entity(lampModel, position, new Vector3(0, 0, 0), scale);

            float lightSourceHeight = position.Y + lightSourceHeightFactor * lamp.Height * scale;
            Light lampLight = new Light(new Vector3(position.X, lightSourceHeight, position.Z),
                lightCoulour, lampAttenuation);

            entities.Add(lamp);
            lights.Add(lampLight);
        }

        private void UpdateLightsBasedOnDayTime()
        {
            float factor = dayTime.TimeOfDayFactor;
            if (dayTime.TimeOfDay == TimeOfDay.Dawn)
            {
                Sun.Colour = new Vector3(factor, factor, factor);

            }
            else if (dayTime.TimeOfDay == TimeOfDay.Morning)
            {
                foreach (var light in Lamps)
                {
                    light.Enabled = false;
                }
            }
            else if (dayTime.TimeOfDay == TimeOfDay.Noon)
            {
                factor = 1 - factor / 2;
                Sun.Colour = new Vector3(factor, factor, factor);
            }
            else if (dayTime.TimeOfDay == TimeOfDay.Evening)
            {
                factor = 0.5f - factor / 2;
                Sun.Colour = new Vector3(factor, factor, factor);
                foreach (var light in Lamps)
                {
                    light.Enabled = true;
                }
            }
        }
    }
}
