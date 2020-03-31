using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AAI.behaviour;
using AAI.Entity.MovingEntities;
using Microsoft.Xna.Framework;

namespace AAI.States
{
    public class Wander:BaseState<MovingEntity>
    {
        private int x;
        public override void Enter(MovingEntity t)
        {
            x = 0;
            t.Behaviours = new List<SteeringBehaviour>()
            {
                new WanderBehaviour(t,20,20),
                new WallAvoidance(t,20),
            };
            t.Velocity = new Vector2();
        }

        public override void Execute(MovingEntity t)
        {
            var distance = t.Pos - t.MyWorld.Target.Pos;
            
            if (x < 500)
            {
                x++;
            }
            else if (distance.Length() < 300)
            {
                t.StateMachine.Changestate(new Attack());
            }

        }

        public override void Exit(MovingEntity t)
        {
        }
    }
}