//using System;
//using System.Collections.Generic;
//using System.Security.Cryptography.X509Certificates;
//using AAI.behaviour;
//using AAI.Entity.MovingEntities;
//using Microsoft.Xna.Framework;

//namespace AAI.Goals
//{
//    public class WanderGoal : BaseGoal
//    {
//        private int x;
//        public void Activate(MovingEntity t)
//        {
//            x = 0;
//            t.Behaviours = new List<SteeringBehaviour>()
//            {
//                new WanderBehaviour(t,20,20),
//                new WallAvoidance(t,20),
//            };
//            t.Velocity = new Vector2();
//        }

//        public int Procces(MovingEntity t)
//        {
//            var distance = t.Pos - t.MyWorld.Target.Pos;

//            if (x < 500)
//            {
//                x++;
//            }
//            else if (distance.Length() < 300)
//            {
//                t.StateMachine.Changestate(new EatGoal());
//            }
//            throw new NotImplementedException();
//        }

//        public void Terminate(MovingEntity t)
//        {
//        }
//    }
//}