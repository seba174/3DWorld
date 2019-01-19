using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using RenderEngine;

namespace ToolBox
{
    public class Loader
    {
        private const string baseTexturePath = "../../../Resources/";
        private readonly List<int> vaos = new List<int>();
        private readonly List<int> vbos = new List<int>();
        private readonly List<int> textures = new List<int>();

        public RawModel LoadToVAO(string objFileName)
        {
            ModelData data = OBJLoader.LoadObjModel(objFileName);
            return LoadToVAO(data.Vertices, data.TextureCoordinates, data.Normals, data.Indices);
        }

        public RawModel LoadToVAO(float[] positions, float[] textureCoords, float[] normals, int[] indicies)
        {
            int vaoID = CreateVAO();

            BindIndiciesBuffer(indicies);
            StoreDataInAttributesList(0, 3, positions);
            StoreDataInAttributesList(1, 2, textureCoords);
            StoreDataInAttributesList(2, 3, normals);
            UnbindVAO();

            return new RawModel(vaoID, indicies.Length);
        }

        public int InitTexture(string fileName)
        {
            var (texture, width, height) = LoadTexture(fileName);

            GL.CreateTextures(TextureTarget.Texture2D, 1, out int textureID);
            GL.TextureStorage2D(textureID, 1, SizedInternalFormat.Rgba32f, width, height);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TextureSubImage2D(textureID, 0, 0, 0, width, height, PixelFormat.Rgba, PixelType.Float, texture);
            textures.Add(textureID);

            return textureID;
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
            foreach (var texture in textures)
            {
                GL.DeleteTexture(texture);
            }
        }

        private int CreateVAO()
        {
            int vaoID = GL.GenVertexArray();
            vaos.Add(vaoID);

            GL.BindVertexArray(vaoID);

            return vaoID;
        }

        private void BindIndiciesBuffer(int[] buffer)
        {
            int vboID = GL.GenBuffer();
            vbos.Add(vboID);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboID);

            GL.BufferData(BufferTarget.ElementArrayBuffer, buffer.Length * sizeof(int), buffer, BufferUsageHint.StaticDraw);
        }

        private void StoreDataInAttributesList(int attributeNumber, int coordinateSize, float[] data)
        {
            int vboID = GL.GenBuffer();
            vbos.Add(vboID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);

            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeNumber, coordinateSize, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void UnbindVAO()
        {
            GL.BindVertexArray(0);
        }

        private (float[] texture, int width, int height) LoadTexture(string fileName)
        {
            (float[] texture, int width, int height) result;

            using (var bmp = (Bitmap)Image.FromFile(baseTexturePath + fileName))
            {
                result.width = bmp.Width;
                result.height = bmp.Height;
                result.texture = new float[result.width * result.height * sizeof(float)];

                int index = 0;
                for (int y = 0; y < result.height; y++)
                {
                    for (int x = 0; x < result.width; x++)
                    {
                        var pixel = bmp.GetPixel(x, y);
                        result.texture[index++] = pixel.R / 255f;
                        result.texture[index++] = pixel.G / 255f;
                        result.texture[index++] = pixel.B / 255f;
                        result.texture[index++] = pixel.A / 255f;
                    }
                }
            }

            return result;
        }
    }
}
