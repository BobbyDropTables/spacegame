using System;
using entity;
using Microsoft.Xna.Framework;

namespace SteeringCS.behaviour
{
    internal class SeekBehaviour : SteeringBehaviour
    {
        // to do
        public SeekBehaviour(MovingEntity me) : base(me)
        {
        }

        public override Vector2 Calculate()
        {
            //var tempTarget      = ME.MyWorld.Target.Pos.Clone();
            //var target          = ME.MyWorld.Target.Pos.Clone();
            //var DesiredVelocity = tempTarget.Sub(ME.Pos);
            //DesiredVelocity.Normalize();
            //DesiredVelocity.Multiply(ME.MaxSpeed);
            //Console.WriteLine(ME.Pos.Distance(target));
            //return ME.Pos.Distance(target) <= 2 ? new Vector2(0, 0) : DesiredVelocity.Sub(ME.Velocity);
            throw new NotImplementedException();
        }
    }
}