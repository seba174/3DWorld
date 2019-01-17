namespace RenderEngine
{
    public class RawModel
    {
        public int VaoID { get; }
        public int VertexCount { get; }

        public RawModel(int vaoID, int vertexCount)
        {
            this.VaoID = vaoID;
            this.VertexCount = vertexCount;
        }
    }
}
