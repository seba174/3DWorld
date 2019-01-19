using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Shaders;
using Terrains;
using Textures;
using ToolBox;

namespace RenderEngine
{
    public class TerrainRenderer
    {
        private TerrainShader shader;

        public TerrainRenderer(TerrainShader shader, Matrix4 projectionMatrix)
        {
            this.shader = shader;
            shader.Start();
            shader.LoadProjectionMatrix(projectionMatrix);
            shader.Stop();
        }

        public void Render(List<Terrain> terrains)
        {
            foreach (var terrain in terrains)
            {
                PrepareTerrain(terrain);
                LoadModelMatrix(terrain);

                GL.DrawElements(BeginMode.Triangles, terrain.Model.VertexCount, DrawElementsType.UnsignedInt, 0);

                UnbindTexturedModel();
            }
        }

        private void PrepareTerrain(Terrain terrain)
        {
            RawModel model = terrain.Model;
            GL.BindVertexArray(model.VaoID);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            ModelTexture texture = terrain.Texture;
            shader.LoadShineVariables(texture.ShineDampler, texture.Reflectivity);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);
        }

        private void UnbindTexturedModel()
        {
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);

            GL.BindVertexArray(0);
        }

        private void LoadModelMatrix(Terrain terrain)
        {
            Matrix4 transformationMatrix = Maths.CreateTransformationMatrix(new Vector3(terrain.X, 0, terrain.Z), new Vector3(0, 0, 0), 1);
            shader.LoadTransformationMatrix(transformationMatrix);
        }
    }
}
