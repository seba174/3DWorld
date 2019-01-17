using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;


namespace RenderEngine
{
    public class Loader
    {

        private readonly List<int> vaos = new List<int>();
        private readonly List<int> vbos = new List<int>();

        public RawModel LoadToVAO(float[] positions)
        {
            int vaoID = CreateVAO();
            StoreDataInAttributesList(0, positions);
            UnbindVAO();

            return new RawModel(vaoID, positions.Length / 3);
        }

        public void CleanUp()
        {
            foreach (var vao in vaos)
            {
                GL.DeleteVertexArray(vao);
            }

            foreach (var vbo in vbos)
            {
                GL.DeleteBuffer(vbo);
            }
        }

        private int CreateVAO()
        {
            int vaoID = GL.GenVertexArray();
            vaos.Add(vaoID);

            GL.BindVertexArray(vaoID);

            return vaoID;
        }

        private void StoreDataInAttributesList(int attributeNumber, float[] data)
        {
            int vboID = GL.GenBuffer();
            vbos.Add(vboID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);

            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeNumber, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void UnbindVAO()
        {
            GL.BindVertexArray(0);
        }


    }
}
