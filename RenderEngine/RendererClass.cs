using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace RenderEngine
{
    public class Renderer
    {
        public void Prepare()
        {
            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void Render(RawModel model)
        {
            GL.BindVertexArray(model.VaoID);

            GL.EnableVertexAttribArray(0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, model.VertexCount);
            GL.DisableVertexAttribArray(0);

            GL.BindVertexArray(0);
        }
    }
}
