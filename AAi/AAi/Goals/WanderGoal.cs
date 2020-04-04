using System.Collections.Generic;
using AAI.behaviour;
using AAI.Entity.MovingEntities;
using Microsoft.Xna.Framework;

namespace AAI.Goals
{
    public class WanderGoal : CompositeGoal
    {
        private int i;

        public WanderGoal(SmartEntity smartEntity)
        {
            i       = 0;
            State   = Statusgoal.inactive;
            this.smartEntity = smartEntity;
            Name    = "Wandering";
        }

        public override void Activate()
        {
            i     = 0;
            State = Statusgoal.active;
            smartEntity.Behaviours = new List<SteeringBehaviour>
            {
                new WanderBehaviour(smartEntity, 30,100),
                new WallAvoidance(smartEntity,15),
                new SeperationBehaviour(smartEntity)
            };
        }

        public override Statusgoal Process()
        {
            if (State == Statusgoal.inactive)
                Activate();

            //count how many times this is reached
            i++;
            //is it more then 10 continue
            if (i > 100)
            {
                State = Statusgoal.completed;
            }

            return State;
        }
    }
}