using OpenTK;

namespace Entities
{
    public class Light
    {
        public Vector3 Position { get; set; }
        public Vector3 Colour { get; set; }
        public Vector3 Attenuation { get; set; } = new Vector3(1, 0, 0);

        public Light(Vector3 position, Vector3 colour)
        {
            Position = position;
            Colour = colour;
        }

        public Light(Vector3 position, Vector3 colour, Vector3 attenuation)
        {
            Attenuation = attenuation;
            Position = position;
            Colour = colour;
        }
    }
}
