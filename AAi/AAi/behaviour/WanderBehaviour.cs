using System;
using System.Security.Cryptography;
using AAI.Entity.MovingEntities;
using Microsoft.Xna.Framework;

namespace AAI.behaviour
{
    internal class WanderBehaviour : SteeringBehaviour
    {
        private readonly float wanderDistance;
        private readonly float wanderRadius;

        public WanderBehaviour(MovingEntity me, float radius, float distance) : base(me)
        {
            wanderRadius   = radius;
            wanderDistance = distance;
        }

        public override Vector2 Calculate()
        {
            var randomdeg = ME.MyWorld.Random.Next(0, 360);
            float  sin    = (float) Math.Sin(MathHelper.ToDegrees(randomdeg));
            float  cos    = (float) Math.Cos(MathHelper.ToDegrees(randomdeg));

            float tx = wanderRadius;
            int   ty = 0;

            Vector2 rotate = new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);

            Vector2 _circlePos = ME.Pos      + ME.Velocity * wanderDistance;
            Vector2 _target    = _circlePos  + rotate;
            return Vector2.Normalize(_target - ME.Pos);
        }
    }
}