using System;
using entity;
using Microsoft.Xna.Framework;

namespace SteeringCS.behaviour
{
    internal class ArriveBehaviour : SteeringBehaviour
    {
        public ArriveBehaviour(MovingEntity me) : base(me)
        {
        }

        public override Vector2 Calculate()
        {
            Vector2 target = new Vector2(ME.MyWorld.Target.Pos.X, ME.MyWorld.Target.Pos.Y);
            var toTarget = Vector2.Subtract(target,ME.Pos);
            //calculate the distance to the target position
            var dist = toTarget.Length();
            if (dist > 0)
            {
                float speed = dist / 2;
                speed = Math.Min(speed, ME.MaxSpeed);
                var desiredVelocity = Vector2.Divide(Vector2.Multiply(toTarget, speed), dist);
                return Vector2.Subtract(desiredVelocity, ME.Velocity);
            }

            return new Vector2(0, 0);
        }
    }
}