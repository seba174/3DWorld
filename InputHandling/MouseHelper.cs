using OpenTK.Input;

namespace InputHandling
{
    public class MouseHelper
    {
        public float WheelDelta { get; private set; }
        public bool LeftButtonPressed { get; private set; }
        public bool RightButtonPressed { get; private set; }
        public int MoveXDelta { get; private set; }
        public int MoveYDelta { get; private set; }

        public void UpdateMouseButtons(MouseButtonEventArgs e)
        {
            LeftButtonPressed = e.Mouse.LeftButton == ButtonState.Pressed ? true : false;
            RightButtonPressed = e.Mouse.RightButton == ButtonState.Pressed ? true : false;
        }

        public void UpdateMouseMove(MouseMoveEventArgs e)
        {
            MoveXDelta = e.XDelta;
            MoveYDelta = e.YDelta;
        }

        public void UpdateMouseWheel(MouseWheelEventArgs e)
        {
            WheelDelta += e.DeltaPrecise;
        }

        public void ResetDeltas()
        {
            WheelDelta = 0;
            MoveXDelta = 0;
            MoveYDelta = 0;
        }
    }
}
