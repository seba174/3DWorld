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

        public Entity(TexturedModel model, Vector3 position, Vector3 rotation, float scale)
        {
            Model = model;
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
    }
}
