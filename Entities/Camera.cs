using OpenTK;

namespace Entities
{
    public class Camera
    {
        public Vector3 Position { get; private set; } = new Vector3(0, 0, 0);
        public float Pitch { get; } = 0;
        public float Yaw { get; } = 0;
        public float Roll { get; } = 0;

        public void Move(bool keyW, bool keyD, bool keyA)
        {
            if (keyA)
            {
                Position += new Vector3(-0.1f, 0, 0);
            }
            if (keyW)
            {
                Position += new Vector3(0, 0, -0.1f);
            }
            if(keyD)
            {
                Position += new Vector3(0.1f, 0, 0);
            }
        }
    }
}
