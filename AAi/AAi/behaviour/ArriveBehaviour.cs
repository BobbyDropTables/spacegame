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
            var toTarget = target - ME.Pos;
            //calculate the distance to the target position
            var dist = toTarget.Length();
            Console.WriteLine(dist);
            if (dist > 0)
            {
                var speed = dist / 2;
                speed = Math.Min(speed, ME.MaxSpeed);
                var desiredVelocity = toTarget *speed /dist;

                return desiredVelocity - ME.Velocity;
            }

            return new Vector2(0, 0);
        }
    }
}