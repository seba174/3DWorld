using System;
using InputHandlings;
using Models;
using OpenTK;

namespace Entities
{
    public class Player : Entity
    {
        private const float RUN_SPEED = 0.018f;
        private const float TURN_SPEED = 0.16f;
        private const float GRAVITY = -0.0005f;
        private const float JUMP_POWER = 0.2f;

        private const float TERRAIN_HEIGHT = 0;

        private float currentSpeed = 0;
        private float currentTurnSpeed = 0;
        private float upwardsSpeed = 0;

        private bool inAir = false;

        public Player(TexturedModel model, Vector3 position, Vector3 rotation, float scale) : base(model, position, rotation, scale)
        {
        }

        public void Move(KeyboardHelper keyboard, long frameTime)
        {
            UpdateSpeeds(keyboard);
            Rotation += new Vector3(0, currentTurnSpeed * frameTime, 0);

            float distance = currentSpeed * frameTime;
            float dx = (float)(distance * Math.Sin(MathHelper.DegreesToRadians(Rotation.Y)));
            float dz = (float)(distance * Math.Cos(MathHelper.DegreesToRadians(Rotation.Y)));
            Position += new Vector3(dx, 0, dz);

            upwardsSpeed += GRAVITY * frameTime;
            Position += new Vector3(0, upwardsSpeed, 0);

            if (Position.Y < TERRAIN_HEIGHT)
            {
                upwardsSpeed = 0;
                Position = new Vector3(Position.X, TERRAIN_HEIGHT, Position.Z);
                inAir = false;
            }
        }

        private void UpdateSpeeds(KeyboardHelper keyboard)
        {
            if (keyboard.W_Pressed)
            {
                currentSpeed = RUN_SPEED;
            }
            else if (keyboard.S_Pressed)
            {
                currentSpeed = -RUN_SPEED;
            }
            else
            {
                currentSpeed = 0;
            }

            if (keyboard.D_Pressed)
            {
                currentTurnSpeed = -TURN_SPEED;
            }
            else if (keyboard.A_Pressed)
            {
                currentTurnSpeed = TURN_SPEED;
            }
            else
            {
                currentTurnSpeed = 0;
            }

            if (keyboard.SpacePressed)
            {
                Jump();
            }
        }

        private void Jump()
        {
            if (!inAir)
            {
                upwardsSpeed = JUMP_POWER;
                inAir = true;
            }
        }
    }
}
