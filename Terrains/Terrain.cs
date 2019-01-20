using OpenTK;
using RenderEngine;
using Textures;
using ToolBox;

namespace Terrains
{
    public class Terrain
    {
        private const string baseResourcesPath = "../../../Resources/";
        private const float Size = 800;
        private const float MaxHeight = 50;

        public float X { get; private set; }
        public float Z { get; private set; }
        public RawModel Model { get; private set; }

        public TerrainTexturePack TexturePack { get; private set; }
        public TerrainTexture BlendMap { get; }

        public Terrain(int gridX, int gridZ, Loader loader, TerrainTexturePack texturePack, TerrainTexture blendMap, string heightMap)
        {
            TexturePack = texturePack;
            BlendMap = blendMap;

            X = gridX * Size;
            Z = gridZ * Size;
            Model = GenerateTerrain(loader, baseResourcesPath + heightMap);
        }

        private RawModel GenerateTerrain(Loader loader, string heightMapPath)
        {
            using (var heightMap = FastBitmapExtensions.GetLockedFastBitmap(heightMapPath))
            {
                int VertexCount = heightMap.Height;

                int count = VertexCount * VertexCount;
                float[] vertices = new float[count * 3];
                float[] normals = new float[count * 3];
                float[] textureCoords = new float[count * 2];
                int[] indices = new int[6 * (VertexCount - 1) * (VertexCount - 1)];
                int vertexPointer = 0;
                for (int i = 0; i < VertexCount; i++)
                {
                    for (int j = 0; j < VertexCount; j++)
                    {
                        vertices[vertexPointer * 3] = j / ((float)VertexCount - 1) * Size - 600;
                        vertices[vertexPointer * 3 + 1] = GetHeight(j, i, heightMap);
                        vertices[vertexPointer * 3 + 2] = i / ((float)VertexCount - 1) * Size - 600;

                        Vector3 normal = CalculateNormal(j, i, heightMap);
                        normals[vertexPointer * 3] = normal.X;
                        normals[vertexPointer * 3 + 1] = normal.Y;
                        normals[vertexPointer * 3 + 2] = normal.Z;
                        textureCoords[vertexPointer * 2] = j / ((float)VertexCount - 1);
                        textureCoords[vertexPointer * 2 + 1] = i / ((float)VertexCount - 1);
                        vertexPointer++;
                    }
                }
                int pointer = 0;
                for (int gz = 0; gz < VertexCount - 1; gz++)
                {
                    for (int gx = 0; gx < VertexCount - 1; gx++)
                    {
                        int topLeft = (gz * VertexCount) + gx;
                        int topRight = topLeft + 1;
                        int bottomLeft = ((gz + 1) * VertexCount) + gx;
                        int bottomRight = bottomLeft + 1;
                        indices[pointer++] = topLeft;
                        indices[pointer++] = bottomLeft;
                        indices[pointer++] = topRight;
                        indices[pointer++] = topRight;
                        indices[pointer++] = bottomLeft;
                        indices[pointer++] = bottomRight;
                    }
                }

                return loader.LoadToVAO(vertices, textureCoords, normals, indices);
            }
        }

        private float GetHeight(int x, int y, FastBitmap heightMap)
        {
            if (x < 0 || x >= heightMap.Height || y < 0 || y >= heightMap.Height)
            {
                return 0;
            }
            var pixel = heightMap.GetPixel(x, y);
            float height = (pixel.R - 128) / 256f;
            height *= MaxHeight;
            return height;
        }

        private Vector3 CalculateNormal(int x, int y, FastBitmap heightMap)
        {
            float heightL = GetHeight(x - 1, y, heightMap);
            float heightR = GetHeight(x + 1, y, heightMap);
            float heightD = GetHeight(x, y - 1, heightMap);
            float heightU = GetHeight(x, y + 1, heightMap);

            Vector3 normal = new Vector3(heightL - heightR, 2f, heightD - heightU);
            normal.Normalize();
            return normal;
        }
    }
}
