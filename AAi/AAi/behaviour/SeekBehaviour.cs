using System;
using AAI.Entity.MovingEntities;
using Microsoft.Xna.Framework;

namespace AAI.behaviour
{
    internal class SeekBehaviour : SteeringBehaviour
    {
        // to do
        public SeekBehaviour(MovingEntity me) : base(me)
        {
        }

        public override Vector2 Calculate()
        {
            var tempTarget      = ME.MyWorld.Target.Pos;
            var target          = ME.MyWorld.Target.Pos;
            var DesiredVelocity = tempTarget-ME.Pos;
            DesiredVelocity.Normalize();
            DesiredVelocity *= ME.MaxSpeed;
            Console.WriteLine(Vector2.Distance(ME.Pos, target));
            return Vector2.Distance(ME.Pos, target) <= 2 ? new Vector2(0, 0) : DesiredVelocity - ME.Velocity;
        }
    }
}