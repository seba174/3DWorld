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
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(1, 0, 0, 1);
        }

        public void Render(RawModel model)
        {
            GL.BindVertexArray(model.VaoID);

            GL.EnableVertexAttribArray(0);
            GL.DrawElements(BeginMode.Triangles, model.VertexCount, DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(0);

            GL.BindVertexArray(0);
        }
    }
}
