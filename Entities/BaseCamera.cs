using InputHandling;
using OpenTK;

namespace Entities
{
    public abstract class BaseCamera
    {
        public Vector3 Position { get; set; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Roll { get; set; }

        public KeyboardHelper Keyboard { get; }
        public MouseHelper Mouse { get; }


        public BaseCamera(KeyboardHelper keyboard, MouseHelper mouse)
        {
            Keyboard = keyboard;
            Mouse = mouse;
        }

        public abstract void Move();
    }
}
