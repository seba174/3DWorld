using System;
using Entities;
using Models;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Shaders;
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

        public Renderer(int displayWidth, int displayHeight, StaticShader shader)
        {
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

        public void Render(Entity entity, StaticShader shader)
        {
            TexturedModel texturedModel = entity.Model;
            RawModel model = texturedModel.RawModel;
            GL.BindVertexArray(model.VaoID);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            Matrix4 transformationMatrix = Maths.CreateTransformationMatrix(entity.Position, entity.Rotation, entity.Scale);
            shader.LoadTransformationMatrix(transformationMatrix);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturedModel.Texture.ID);
            GL.DrawElements(BeginMode.Triangles, model.VertexCount, DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);

            GL.BindVertexArray(0);
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
