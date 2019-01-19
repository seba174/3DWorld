using System.Collections.Generic;
using Entities;
using Models;
using Shaders;

namespace RenderEngine
{
    public class MasterRenderer
    {
        private StaticShader shader;
        private Renderer renderer;
        private Dictionary<TexturedModel, List<Entity>> entities = new Dictionary<TexturedModel, List<Entity>>();

        public MasterRenderer(int height, int widht)
        {
            shader = new StaticShader();
            renderer = new Renderer(widht, height, shader);
        }

        public void Render(Light sun, Camera camera)
        {
            renderer.Prepare();
            shader.Start();

            shader.LoadLight(sun);
            shader.LoadViewMatrix(camera);

            renderer.Render(entities);

            shader.Stop();
            entities.Clear();
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

        public void CleanUp()
        {
            shader.CleanUp();
        }
    }
}
