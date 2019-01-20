using InputHandling;
using OpenTK;

namespace Entities
{
    public abstract class BaseCamera
    {
        public Vector3 Position { get; protected set; }
        public float Pitch { get; protected set; }
        public float Yaw { get; protected set; }
        public float Roll { get; protected set; }

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
