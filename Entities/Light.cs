using OpenTK;

namespace Entities
{
    public class Light
    {
        public Vector3 Position { get; set; }
        public Vector3 Colour { get; set; }

        public Light(Vector3 position, Vector3 colour)
        {
            Position = position;
            Colour = colour;
        }
    }
}
