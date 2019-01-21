using OpenTK;

namespace Entities
{
    public class Light
    {
        public Vector3 Position { get; set; }

        private Vector3 colour;
        public Vector3 Colour
        {
            get => Enabled ? colour : new Vector3(0, 0, 0);
            set => colour = value;
        }
        public Vector3 Attenuation { get; set; } = new Vector3(1, 0, 0);
        public float AdditionalAngle { get; set; }
        public Vector3 ConeDirection { get; set; }
        public Vector2 Angles { get; set; } = new Vector2(-1, -1);
        public bool Enabled { get; set; } = true;

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
