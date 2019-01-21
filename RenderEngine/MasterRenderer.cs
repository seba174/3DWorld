using System;
using System.Collections.Generic;
using Entities;
using Models;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Shaders;
using Skybox;
using Terrains;
using ToolBox;

namespace RenderEngine
{
    public class MasterRenderer
    {
        private const float FOV = 70;
        private const float NEAR_PLANE = 0.1f;
        private const float FAR_PLANE = 1000;
        private static Vector3 FogColour = new Vector3(0.5444f, 0.62f, 0.69f);

        private readonly int height, width;

        private EntityShader shader;
        private EntityRenderer renderer;

        private TerrainShader terrainShader;
        private TerrainRenderer terrainRenderer;

        private SkyboxRenderer skyboxRenderer;

        private Matrix4 projectionMatrix;
        private Dictionary<TexturedModel, List<Entity>> entities = new Dictionary<TexturedModel, List<Entity>>();
        private List<Terrain> terrains = new List<Terrain>();

        public MasterRenderer(int height, int width, Loader loader)
        {
            this.height = height;
            this.width = width;

            EnableCulling();

            shader = new EntityShader();
            terrainShader = new TerrainShader();

            CreateProjectionMatrix();

            renderer = new EntityRenderer(shader, projectionMatrix);
            terrainRenderer = new TerrainRenderer(terrainShader, projectionMatrix);
            skyboxRenderer = new SkyboxRenderer(loader, projectionMatrix);
        }

        public void Render(List<Light> lights, BaseCamera camera, long frameTime)
        {
            Prepare();

            shader.Start();
            shader.LoadSkyColour(FogColour);
            shader.LoadLights(lights);
            shader.LoadViewMatrix(camera);
            renderer.Render(entities);
            shader.Stop();

            terrainShader.Start();
            terrainShader.LoadSkyColour(FogColour);
            terrainShader.LoadLights(lights);
            terrainShader.LoadViewMatrix(camera);
            terrainRenderer.Render(terrains);
            terrainShader.Stop();

            skyboxRenderer.Render(camera, FogColour, frameTime);

            entities.Clear();
            terrains.Clear();
        }

        public void ProcessEntity(Entity entity)
        {
            TexturedModel entityModel = entity.Model;

            if (!entities.TryGetValue(entityModel, out var batch))
            {
                var newBatch = new List<Entity>() { entity };
                entities.Add(entityModel, newBatch);
            }
            else
            {
                batch.Add(entity);
            }
        }

        public static void EnableCulling()
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
        }

        public static void DisableCulling()
        {
            GL.Disable(EnableCap.CullFace);
        }

        public void ProcessTerrain(Terrain terrain)
        {
            terrains.Add(terrain);
        }

        public void Prepare()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(FogColour.X, FogColour.Y, FogColour.Z, 1);
        }

        public void CleanUp()
        {
            shader.CleanUp();
            terrainShader.CleanUp();
            skyboxRenderer.CleanUp();
        }

        private void CreateProjectionMatrix()
        {
            float aspectRatio = width / (float)height;
            float y_scale = (float)(1f / Math.Tan(MathHelper.DegreesToRadians(FOV / 2f)) * aspectRatio);
            float x_scale = y_scale / aspectRatio;
            float frustum_lenght = FAR_PLANE - NEAR_PLANE;

            projectionMatrix = new Matrix4
            {
                M11 = x_scale,
                M22 = y_scale,
                M33 = -((FAR_PLANE + NEAR_PLANE) / frustum_lenght),
                M34 = -1,
                M43 = -(2 * NEAR_PLANE * FAR_PLANE / frustum_lenght),
                M44 = 0
            };
        }
    }
}
