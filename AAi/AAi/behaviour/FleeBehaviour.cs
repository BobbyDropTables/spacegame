using System;
using AAI.Entity.MovingEntities;
using Microsoft.Xna.Framework;

namespace AAI.behaviour
{
    class FleeBehaviour : SteeringBehaviour
    {
        public FleeBehaviour(MovingEntity me) : base(me)
        {
        }

        public override Vector2 Calculate()
        {
            var tempPos      = ME.Pos;
            var target          = ME.MyWorld.Target.Pos;
            var DesiredVelocity = tempPos- target;
            DesiredVelocity.Normalize();
            DesiredVelocity *= ME.MaxSpeed;
            Console.WriteLine(Vector2.Distance(ME.Pos, target));
            return Vector2.Distance(ME.Pos,target) <= 2 ? new Vector2(0, 0) : DesiredVelocity - ME.Velocity;
        }
    }
}


