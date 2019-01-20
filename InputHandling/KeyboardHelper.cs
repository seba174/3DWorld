using OpenTK.Input;

namespace InputHandlings
{
    public class KeyboardHelper
    {
        public bool W_Pressed { get; private set; }
        public bool A_Pressed { get; private set; }
        public bool S_Pressed { get; private set; }
        public bool D_Pressed { get; private set; }
        public bool UpArrowPressed { get; private set; }
        public bool LeftArrowPressed { get; private set; }
        public bool RightArrowPressed { get; private set; }
        public bool DownArrowPressed { get; private set; }
        public bool SpacePressed { get; private set; }

        public void Update(KeyboardKeyEventArgs e, bool pressed)
        {
            switch (e.Key)
            {
                case Key.W:
                    W_Pressed = pressed; break;
                case Key.A:
                    A_Pressed = pressed; break;
                case Key.S:
                    S_Pressed = pressed; break;
                case Key.D:
                    D_Pressed = pressed; break;
                case Key.Up:
                    UpArrowPressed = pressed; break;
                case Key.Left:
                    LeftArrowPressed = pressed; break;
                case Key.Right:
                    RightArrowPressed = pressed; break;
                case Key.Down:
                    DownArrowPressed = pressed; break;
                case Key.Space:
                    SpacePressed = pressed; break;
            }
        }
    }
}
