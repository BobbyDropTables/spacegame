//using System.Collections.Generic;
//using AAI.behaviour;
//using AAI.Entity.MovingEntities;
//using Microsoft.Xna.Framework;

//namespace AAI.Goals
//{
//    public class RestGoal : BaseGoal
//    {
//        public void Activate(MovingEntity t)
//        {
//            t.Behaviours = new List<SteeringBehaviour>()
//            {

//            };
//            t.Velocity = new Vector2();
//        }

//        public int Procces(MovingEntity t)
//        {
//            if (t.energy < 1000)
//            {
//                t.energy += 10;
//            }
//            else
//            {
//                t.StateMachine.Changestate(new WanderGoal());
//            }

//        }

//        public void Terminate(MovingEntity t)
//        {
//            //t.Behaviours = new List<SteeringBehaviour>
//            //{
//            //    //new ArriveBehaviour(this),
//            //    new WanderBehaviour(t,90,100),
//            //    new WallAvoidance(t, 15),
//            //};
//        }
//    }
//}