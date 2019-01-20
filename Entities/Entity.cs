using Models;
using OpenTK;

namespace Entities
{
    public class Entity
    {
        public TexturedModel Model { get; private set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public float Scale { get; set; }

        public int TextureIndex { get; }

        public Entity(TexturedModel model, Vector3 position, Vector3 rotation, float scale)
        {
            Model = model;
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Entity(TexturedModel model, int index, Vector3 position, Vector3 rotation, float scale)
        {
            TextureIndex = index;
            Model = model;
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public float Height => Model.Height * Scale;

        public float GetTextureXOffset()
        {
            int column = TextureIndex % Model.Texture.NumberOfRows;
            return (float)column / Model.Texture.NumberOfRows;
        }

        public float GetTextureYOffset()
        {
            int row = TextureIndex / Model.Texture.NumberOfRows;
            return (float)row / Model.Texture.NumberOfRows;
        }
    }
}
