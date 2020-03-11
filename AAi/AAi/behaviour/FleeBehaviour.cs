using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.entity;
using Game.entity.MovingEntities;
using Microsoft.Xna.Framework;

namespace Game.behaviour
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


