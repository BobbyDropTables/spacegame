using System;
using entity;
using Microsoft.Xna.Framework;

namespace SteeringCS.behaviour
{
    internal class FleeBehaviour : SteeringBehaviour
    {
        public FleeBehaviour(MovingEntity me) : base(me)
        {
        }

        public override Vector2 Calculate()
        {
            //var tempPos      = ME.Pos.Clone();
            //var target          = ME.MyWorld.Target.Pos.Clone();
            //var DesiredVelocity = tempPos.Sub(target);
            //DesiredVelocity.Normalize();
            //DesiredVelocity.Multiply(ME.MaxSpeed);
            //Console.WriteLine(ME.Pos.Distance(target));
            //return ME.Pos.Distance(target) <= 2 ? new Vector2D(0, 0) : DesiredVelocity.Sub(ME.Velocity);
            throw new NotImplementedException();
        }
    }
}