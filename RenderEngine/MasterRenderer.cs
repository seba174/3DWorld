using System;
using System.Collections.Generic;
using Entities;
using Models;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Shaders;
using Terrains;

namespace RenderEngine
{
    public class MasterRenderer
    {
        private const float FOV = 70;
        private const float NEAR_PLANE = 0.1f;
        private const float FAR_PLANE = 1000;
        private static Vector3 SkyColour = new Vector3(0.5f, 0.5f, 0.5f);

        private int height, width;

        private StaticShader shader;
        private EntityRenderer renderer;

        private TerrainShader terrainShader;
        private TerrainRenderer terrainRenderer;

        private Matrix4 projectionMatrix;
        private Dictionary<TexturedModel, List<Entity>> entities = new Dictionary<TexturedModel, List<Entity>>();
        private List<Terrain> terrains = new List<Terrain>();

        public MasterRenderer(int height, int width)
        {
            this.height = height;
            this.width = width;

            EnableCulling();

            shader = new StaticShader();
            terrainShader = new TerrainShader();

            CreateProjectionMatrix();

            renderer = new EntityRenderer(shader, projectionMatrix);
            terrainRenderer = new TerrainRenderer(terrainShader, projectionMatrix);
        }

        public void Render(Light sun, Camera camera)
        {
            Prepare();

            shader.Start();
            shader.LoadSkyColour(SkyColour);
            shader.LoadLight(sun);
            shader.LoadViewMatrix(camera);
            renderer.Render(entities);
            shader.Stop();

            terrainShader.Start();
            shader.LoadSkyColour(SkyColour);
            terrainShader.LoadLight(sun);
            terrainShader.LoadViewMatrix(camera);
            terrainRenderer.Render(terrains);
            terrainShader.Stop();

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
            GL.ClearColor(SkyColour.X, SkyColour.Y, SkyColour.Z, 1);
        }

        public void CleanUp()
        {
            shader.CleanUp();
            terrainShader.CleanUp();
        }

        private void CreateProjectionMatrix()
        {
            float aspectRatio = width / (float)height;
            float y_scale = (float)((1f / Math.Tan(MathHelper.DegreesToRadians(FOV / 2f))) * aspectRatio);
            float x_scale = y_scale / aspectRatio;
            float frustum_lenght = FAR_PLANE - NEAR_PLANE;

            projectionMatrix = new Matrix4();
            projectionMatrix.M11 = x_scale;
            projectionMatrix.M22 = y_scale;
            projectionMatrix.M33 = -((FAR_PLANE + NEAR_PLANE) / frustum_lenght);
            projectionMatrix.M34 = -1;
            projectionMatrix.M43 = -((2 * NEAR_PLANE * FAR_PLANE) / frustum_lenght);
            projectionMatrix.M44 = 0;
        }
    }
}
