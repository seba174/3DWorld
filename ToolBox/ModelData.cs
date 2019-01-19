
namespace ToolBox
{
    public class ModelData
    {
        public float[] Vertices { get; }
        public float[] TextureCoordinates { get; }
        public float[] Normals { get; }
        public int[] Indices { get; }
        public float FurthestPoint { get; }

        public ModelData(float[] vertices, float[] textureCoordinates, float[] normals, int[] indices, float furthestPoint)
        {
            Vertices = vertices;
            TextureCoordinates = textureCoordinates;
            Normals = normals;
            Indices = indices;
            FurthestPoint = furthestPoint;
        }
    }
}
