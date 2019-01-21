using InputHandling;

namespace Entities
{
    public class FirstPersonCamera : ThirdPersonCamera
    {
        public FirstPersonCamera(KeyboardHelper keyboard, MouseHelper mouse, Entity entity) : base(keyboard, mouse, entity)
        {
            base.distanceFromEntity = 0;
            base.angleAroundEntity = 0;
        }

        public override void Move()
        {
            float horizontalDistance = CalculateHorizontalDistance();
            float verticalDistance = CalculateVerticalDistance();
            CalculateCameraPosition(horizontalDistance, verticalDistance);

            Yaw = 180 - (entity.Rotation.Y);
        }
    }
}
