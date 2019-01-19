namespace RenderEngine
{
    public class RawModel
    {
        public int VaoID { get; }
        public int VertexCount { get; }

        public RawModel(int vaoID, int vertexCount)
        {
            VaoID = vaoID;
            VertexCount = vertexCount;
        }
    }
}
