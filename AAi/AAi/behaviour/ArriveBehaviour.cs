using System;
using AAI.Entity.MovingEntities;
using Microsoft.Xna.Framework;

namespace AAI.behaviour
{
    internal class ArriveBehaviour : SteeringBehaviour
    {
        public ArriveBehaviour(MovingEntity me) : base(me)
        {
        }

        public override Vector2 Calculate()
        {

            var toTarget = ME.MyWorld.Target.Pos - ME.Pos;
            //calculate the distance to the target position
            var dist = toTarget.Length();
            if (dist > 1)
            {
                var speed = dist / 50;
                speed = Math.Min(speed, ME.MaxSpeed);
                var desiredVelocity = (toTarget * speed) / dist;
                return desiredVelocity- ME.Velocity;
            }

            return new Vector2(0, 0);
        }
    }
}