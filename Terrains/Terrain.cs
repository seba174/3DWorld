using RenderEngine;
using Textures;
using ToolBox;

namespace Terrains
{
    public class Terrain
    {
        private const float Size = 800;
        private const int VertexCount = 128;

        public float X { get; private set; }
        public float Z { get; private set; }
        public RawModel Model { get; private set; }

        public TerrainTexturePack TexturePack { get; private set; }
        public TerrainTexture BlendMap { get; }

        public Terrain(int gridX, int gridZ, Loader loader, TerrainTexturePack texturePack, TerrainTexture blendMap)
        {
            TexturePack = texturePack;
            BlendMap = blendMap;

            X = gridX * Size;
            Z = gridZ * Size;
            Model = GenerateTerrain(loader);
        }

        private RawModel GenerateTerrain(Loader loader)
        {
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
                    vertices[vertexPointer * 3 + 1] = 0;
                    vertices[vertexPointer * 3 + 2] = i / ((float)VertexCount - 1) * Size - 600;
                    normals[vertexPointer * 3] = 0;
                    normals[vertexPointer * 3 + 1] = 1;
                    normals[vertexPointer * 3 + 2] = 0;
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
}
