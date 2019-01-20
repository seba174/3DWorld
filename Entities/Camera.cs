using InputHandlings;
using OpenTK;

namespace Entities
{
    public class Camera
    {
        public Vector3 Position { get; private set; } = new Vector3(0, 7, 0);
        public float Pitch { get; } = 10;
        public float Yaw { get; } = 0;
        public float Roll { get; } = 0;

        public void Move(KeyboardHelper keyboard)
        {
            if (keyboard.LeftArrowPressed)
            {
                Position += new Vector3(-0.3f, 0, 0);
            }
            if (keyboard.UpArrowPressed)
            {
                Position += new Vector3(0, 0, -0.3f);
            }
            if (keyboard.RightArrowPressed)
            {
                Position += new Vector3(0.3f, 0, 0);
            }
            if (keyboard.DownArrowPressed)
            {
                Position += new Vector3(0, 0, 0.3f);
            }
        }
    }
}
