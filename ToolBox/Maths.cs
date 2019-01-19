using Entities;
using OpenTK;

namespace ToolBox
{
    public class Maths
    {
        public static Matrix4 CreateTransformationMatrix(Vector3 translation, Vector3 rotation, float scale)
        {
            Matrix4 matrix = Matrix4.Identity;

            matrix *= Matrix4.CreateScale(scale, scale, scale);
            matrix *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));

            matrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            matrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X));
            matrix *= Matrix4.CreateTranslation(translation);

            return matrix;
        }

        public static Matrix4 CreateViewMatrix(Camera camera)
        {
            Matrix4 viewMatrix = Matrix4.Identity;

            viewMatrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(camera.Pitch));
            viewMatrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(camera.Yaw));

            Vector3 negativeCameraPosition = -camera.Position;
            viewMatrix *= Matrix4.CreateTranslation(negativeCameraPosition);

            return viewMatrix;
        }
    }
}
