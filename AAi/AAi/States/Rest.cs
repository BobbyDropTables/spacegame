using System.Collections.Generic;
using AAI.behaviour;
using AAI.Entity.MovingEntities;
using Microsoft.Xna.Framework;

namespace AAI.States
{
    public class Rest : BaseState<MovingEntity>
    {
        public override void Enter(MovingEntity t)
        {
            t.Behaviours = new List<SteeringBehaviour>()
            {
                
            };
            t.Velocity = new Vector2();
        }

        public override void Execute(MovingEntity t)
        {
            if (t.energy < 100)
            {
                t.energy++;
            }
            else
            {
                t.StateMachine.Changestate(new Attack());
            }

        }

        public override void Exit(MovingEntity t)
        {
            //t.Behaviours = new List<SteeringBehaviour>
            //{
            //    //new ArriveBehaviour(this),
            //    new WanderBehaviour(t,90,100),
            //    new WallAvoidance(t, 15),
            //};
        }
    }
}