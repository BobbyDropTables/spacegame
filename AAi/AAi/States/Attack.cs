using System;
using System.Collections.Generic;
using AAI.behaviour;
using AAI.Entity.MovingEntities;

namespace AAI.States
{
    public class Attack : BaseState<MovingEntity>
    {
        public override void Enter(MovingEntity t)
        {
            t.Behaviours = new List<SteeringBehaviour>()
            {
                new ArriveBehaviour(t)
            };
        }

        public override void Execute(MovingEntity t)
        {
            var distance = t.Pos - t.MyWorld.Target.Pos;
            if (distance.Length() < 1)
            {
                t.StateMachine.Changestate(new Wander());
                return;
            }
            if (t.energy > 0)
            {
                t.energy--;
            }
            else 
            {
                t.StateMachine.Changestate(new Rest());
            }

        }

        public override void Exit(MovingEntity t)
        {
            t.Behaviours = new List<SteeringBehaviour>
            {
                //new ArriveBehaviour(this),
                new WanderBehaviour(t,90,100),
                new WallAvoidance(t, 15),
            };
        }
    }
}