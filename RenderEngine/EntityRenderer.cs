using System;
using System.Collections.Generic;
using Entities;
using Models;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Shaders;
using Textures;
using ToolBox;

namespace RenderEngine
{
    public class EntityRenderer
    {
        private StaticShader shader;

        public EntityRenderer(StaticShader shader, Matrix4 projectionMatrix)
        {
            this.shader = shader;

            shader.Start();
            shader.LoadProjectionMatrix(projectionMatrix);
            shader.Stop();
        }

        public void Render(Dictionary<TexturedModel, List<Entity>> entities)
        {
            foreach (var model in entities)
            {
                PrepareTexturedModel(model.Key);
                foreach (var entity in model.Value)
                {
                    PrepareInstance(entity);
                    GL.DrawElements(BeginMode.Triangles, model.Key.RawModel.VertexCount, DrawElementsType.UnsignedInt, 0);
                }
                UnbindTexturedModel();
            }
        }

        private void PrepareTexturedModel(TexturedModel texturedModel)
        {
            RawModel model = texturedModel.RawModel;
            GL.BindVertexArray(model.VaoID);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            ModelTexture texture = texturedModel.Texture;
            if (texture.HasTransparency)
            {
                MasterRenderer.DisableCulling();
            }
            shader.LoadFakeLightingVariable(texture.UseFakeLightning);
            shader.LoadShineVariables(texture.ShineDampler, texture.Reflectivity);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturedModel.Texture.ID);

        }

        private void UnbindTexturedModel()
        {
            MasterRenderer.EnableCulling();

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);

            GL.BindVertexArray(0);
        }

        private void PrepareInstance(Entity entity)
        {
            Matrix4 transformationMatrix = Maths.CreateTransformationMatrix(entity.Position, entity.Rotation, entity.Scale);
            shader.LoadTransformationMatrix(transformationMatrix);
        }
    }
}
