using OpenTK;

namespace ToolBox
{
    public class Vertex
    {
        private const int NoIndex = -1;

        public Vector3 Position { get; }
        public int TextureIndex { get; set; } = NoIndex;
        public int NormalIndex { get; set; } = NoIndex;
        public Vertex DuplicateVertex { get; set; }
        public int Index { get; }
        public float Length { get; }

        public Vertex(int index, Vector3 position)
        {
            Index = index;
            Position = position;
            Length = position.Length;
        }

        public bool IsSet()
        {
            return TextureIndex != NoIndex && NormalIndex != NoIndex;
        }

        public bool HasSameTextureAndNormal(int textureIndexOther, int normalIndexOther)
        {
            return textureIndexOther == TextureIndex && normalIndexOther == NormalIndex;
        }
    }
}
