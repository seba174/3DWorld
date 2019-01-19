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
    public class Renderer
    {
        private const float FOV = 70;
        private const float NEAR_PLANE = 0.1f;
        private const float FAR_PLANE = 1000;

        private int width;
        private int height;
        private Matrix4 projectionMatrix;
        private StaticShader shader;

        public Renderer(int displayWidth, int displayHeight, StaticShader shader)
        {
            this.shader = shader;
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            width = displayWidth;
            height = displayHeight;
            CreateProjectionMatrix();

            shader.Start();
            shader.LoadProjectionMatrix(projectionMatrix);
            shader.Stop();
        }

        public void Prepare()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0, 0, 0, 1);
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
            shader.LoadShineVariables(texture.ShineDampler, texture.Reflectivity);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturedModel.Texture.ID);

        }

        private void UnbindTexturedModel()
        {
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
