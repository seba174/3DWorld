using System;
using InputHandling;
using OpenTK;

namespace Entities
{
    public class ThirdPersonCamera : BaseCamera
    {
        protected float distanceFromEntity = 25;
        protected float angleAroundEntity = 0;

        protected readonly Entity entity;

        public ThirdPersonCamera(KeyboardHelper keyboard, MouseHelper mouse, Entity entity)
            : base(keyboard, mouse)
        {
            this.entity = entity;
            Pitch = 10;
        }

        public override void Move()
        {
            CalculateZoom();
            CalculatePitch();
            CalculateAngleAroundEntity();

            float horizontalDistance = CalculateHorizontalDistance();
            float verticalDistance = CalculateVerticalDistance();
            CalculateCameraPosition(horizontalDistance, verticalDistance);

            Yaw = 180 - (entity.Rotation.Y + angleAroundEntity);
        }

        protected void CalculateZoom()
        {
            float zoomLevel = Mouse.WheelDelta * 0.5f;
            distanceFromEntity -= zoomLevel;
        }

        protected void CalculatePitch()
        {
            if (Mouse.RightButtonPressed)
            {
                float pitchChange = Mouse.MoveYDelta * 0.1f;
                Pitch -= pitchChange;
            }
        }

        protected void CalculateAngleAroundEntity()
        {
            if (Mouse.LeftButtonPressed)
            {
                float angleChange = Mouse.MoveXDelta * 0.3f;
                angleAroundEntity -= angleChange;
            }
        }

        protected float CalculateHorizontalDistance()
        {
            return (float)(distanceFromEntity * Math.Cos(MathHelper.DegreesToRadians(Pitch)));
        }

        protected float CalculateVerticalDistance()
        {
            return (float)(distanceFromEntity * Math.Sin(MathHelper.DegreesToRadians(Pitch)));
        }

        protected void CalculateCameraPosition(float horizontalDistance, float verticalDistance)
        {
            float theta = entity.Rotation.Y + angleAroundEntity;
            float offsetX = (float)(horizontalDistance * Math.Sin(MathHelper.DegreesToRadians(theta)));
            float offsetZ = (float)(horizontalDistance * Math.Cos(MathHelper.DegreesToRadians(theta)));

            float factor = 0.75f;
            if (distanceFromEntity <= 0)
            {
                factor = 1;
            }

            Position = entity.Position + new Vector3(-offsetX, entity.Height * factor + verticalDistance, -offsetZ);
        }
    }
}
