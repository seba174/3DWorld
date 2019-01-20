using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Models;
using OpenTK.Graphics.OpenGL4;
using RenderEngine;
using Textures;
using Utilities;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace ToolBox
{
    public class Loader
    {
        private const int LevelOfMipmaping = 5;
        
        private readonly List<int> vaos = new List<int>();
        private readonly List<int> vbos = new List<int>();
        private readonly List<int> textures = new List<int>();

        public TexturedModel CreateTexturedModel(string objFileName, string textureFileName, int numberOfRows = 1)
        {
            ModelData data = OBJLoader.LoadObjModel(objFileName);
            var rawModel = LoadToVAO(data.Vertices, data.TextureCoordinates, data.Normals, data.Indices);
            var modelTexture = new ModelTexture(InitTexture(textureFileName))
            {
                NumberOfRows = numberOfRows
            };
            return new TexturedModel(rawModel, data.Height, modelTexture);
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

        public RawModel LoadToVAO(float[] positions, int dimensions)
        {
            int vaoID = CreateVAO();
            StoreDataInAttributesList(0, dimensions, positions);
            UnbindVAO();
            return new RawModel(vaoID, positions.Length / dimensions);
        }

        public int InitTexture(string fileName)
        {
            var (texture, width, height) = LoadTexture(fileName);

            GL.CreateTextures(TextureTarget.Texture2D, 1, out int textureID);
            GL.TextureStorage2D(textureID, LevelOfMipmaping, SizedInternalFormat.Rgba32f, width, height);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TextureSubImage2D(textureID, 0, 0, 0, width, height, PixelFormat.Rgba, PixelType.Float, texture);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TextureParameter(textureID, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);

            GL.GetFloat((GetPName)OpenTK.Graphics.ES30.ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out float maxAnsio);
            GL.TextureParameter(textureID, (TextureParameterName)OpenTK.Graphics.ES30.ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, maxAnsio);

            //GL.TextureParameter(textureID, TextureParameterName.TextureLodBias, -1f);

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

            using (var fastBitmap = FastBitmapExtensions.GetLockedFastBitmap(Constants.BaseResourcesPath + fileName + Constants.TextureFileExtension))
            {
                result.width = fastBitmap.Width;
                result.height = fastBitmap.Height;
                result.texture = new float[result.width * result.height * sizeof(float)];

                int index = 0;
                for (int y = 0; y < result.height; y++)
                {
                    for (int x = 0; x < result.width; x++)
                    {
                        var pixel = fastBitmap.GetPixel(x, y);
                        result.texture[index++] = pixel.R / 255f;
                        result.texture[index++] = pixel.G / 255f;
                        result.texture[index++] = pixel.B / 255f;
                        result.texture[index++] = pixel.A / 255f;
                    }
                }
            }

            return result;
        }

        public int LoadCubeMap(string[] textureFiles)
        {
            int texID = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, texID);

            for (int i = 0; i < textureFiles.Length; i++)
            {
                Bitmap image = new Bitmap(Constants.BaseResourcesPath + textureFiles[i] + Constants.TextureFileExtension);

                BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, data.Width, data.Height,
                    0, PixelFormat.Rgba, PixelType.UnsignedByte, data.Scan0);

                image.UnlockBits(data);
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (float)All.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (float)All.Linear);

            textures.Add(texID);
            return texID;
        }
    }
}
