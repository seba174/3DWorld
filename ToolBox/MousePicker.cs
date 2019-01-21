using Entities;
using InputHandling;
using OpenTK;

namespace ToolBox
{
    public class MousePicker
    {
        public Vector3 CurrentRay { get; private set; }
        private Matrix4 ProjectionMatrix;
        private Matrix4 ViewMatrix;
        private MouseHelper mouse;
        private ScreenHelper screen;

        public MousePicker(Matrix4 projectionMatrix, MouseHelper mouse, ScreenHelper screen)
        {
            ProjectionMatrix = projectionMatrix;
            this.mouse = mouse;
            this.screen = screen;
        }

        public void Update(BaseCamera camera)
        {
            ViewMatrix = Maths.CreateViewMatrix(camera);
            CurrentRay = CalculateMouseRay();
        }

        private Vector3 CalculateMouseRay()
        {
            Vector2 normalizedCoordinates = GetNormalizedDeviceCoordinates();
            Vector4 clipCoordinates = new Vector4(normalizedCoordinates.X, normalizedCoordinates.Y, -1, 1);
            Vector4 eyeSpaceCoordinates = ToEyeCoordinates(clipCoordinates);
            Vector3 worldRay = ToWorldCoordinates(eyeSpaceCoordinates);
            return worldRay;
        }

        private Vector3 ToWorldCoordinates(Vector4 eyeCoordinates)
        {
            Matrix4 invertedViewMatrix = Matrix4.Invert(ViewMatrix);
            Vector4 worldCoordinates = invertedViewMatrix * eyeCoordinates;
            Vector3 mouseRay = new Vector3(worldCoordinates.X, worldCoordinates.Y, worldCoordinates.Z);
            mouseRay.Normalize();

            return mouseRay;
        }

        private Vector4 ToEyeCoordinates(Vector4 clipCoordinates)
        {
            Matrix4 invertedProjectionMatrix = Matrix4.Invert(ProjectionMatrix);
            Vector4 eyeCoordinates = invertedProjectionMatrix * clipCoordinates;
            return new Vector4(eyeCoordinates.X, eyeCoordinates.Y, -1, 0);
        }

        private Vector2 GetNormalizedDeviceCoordinates()
        {
            float x = 2 * mouse.X / screen.Width - 1;
            float y = 2 * mouse.Y / screen.Height - 1;
            return new Vector2(x, y);
        }
    }
}
