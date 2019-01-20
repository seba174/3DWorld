using InputHandling;
using OpenTK;

namespace Entities
{
    public class Camera : BaseCamera
    {
        public Camera(KeyboardHelper keyboard, MouseHelper mouse)
            : base(keyboard, mouse)
        {
            Position = new Vector3(0, 10, 0);
            Pitch = 10;
        }

        public override void Move()
        {
            if (Keyboard.LeftArrowPressed)
            {
                Position += new Vector3(-0.3f, 0, 0);
            }
            if (Keyboard.UpArrowPressed)
            {
                Position += new Vector3(0, 0, -0.3f);
            }
            if (Keyboard.RightArrowPressed)
            {
                Position += new Vector3(0.3f, 0, 0);
            }
            if (Keyboard.DownArrowPressed)
            {
                Position += new Vector3(0, 0, 0.3f);
            }
        }
    }
}
