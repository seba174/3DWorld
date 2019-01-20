using Entities;
using OpenTK;

namespace ToolBox
{
    public class Maths
    {
        public static float BarryCentric(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pos)
        {
            float det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
            float l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Y - p3.Z)) / det;
            float l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Y - p3.Z)) / det;
            float l3 = 1.0f - l1 - l2;
            return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
        }

        public static Matrix4 CreateTransformationMatrix(Vector3 translation, Vector3 rotation, float scale)
        {
            Matrix4 matrix = Matrix4.Identity;
            matrix *= Matrix4.CreateScale(scale, scale, scale);

            matrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X));
            matrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
            matrix *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));

            matrix *= Matrix4.CreateTranslation(translation);

            return matrix;
        }

        public static Matrix4 CreateViewMatrix(BaseCamera camera)
        {
            Matrix4 viewMatrix = Matrix4.Identity;
            Vector3 negativeCameraPosition = -camera.Position;

            viewMatrix *= Matrix4.CreateTranslation(negativeCameraPosition);

            viewMatrix *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(camera.Yaw));
            viewMatrix *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(camera.Pitch));

            return viewMatrix;
        }
    }
}
