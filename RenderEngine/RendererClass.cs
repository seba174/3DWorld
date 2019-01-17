using Models;
using OpenTK.Graphics.OpenGL4;

namespace RenderEngine
{
    public class Renderer
    {
        public void Prepare()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(1, 0, 0, 1);
        }

        public void Render(TexturedModel texturedModel)
        {
            RawModel model = texturedModel.RawModel;
            GL.BindVertexArray(model.VaoID);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturedModel.Texture.ID);
            GL.DrawElements(BeginMode.Triangles, model.VertexCount, DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);

            GL.BindVertexArray(0);
        }
    }
}
