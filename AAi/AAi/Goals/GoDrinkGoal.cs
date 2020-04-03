using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using Microsoft.Xna.Framework;

namespace AAI.Goals
{
    class GoDrinkGoal : CompositeGoal
    {
        private Target Target;
        public GoDrinkGoal(SmartEntity smartEntity, Target target)
        {
            State    = Statusgoal.inactive;
            this.smartEntity  = smartEntity;
            Name     = "Go Drink";
            Target   = target;
            SubGoals = new List<CompositeGoal>();
        }

        public override void Activate()
        {
            State  = Statusgoal.active;
            SubGoals.Clear();
            SubGoals.Add(new PathFollowingGoal(smartEntity, Target));
            SubGoals.Add(new DrinkGoal(smartEntity, Target));
        }

        public override Statusgoal Process()
        {
            if (State == Statusgoal.inactive)
            {
                Activate();
            }

            Statusgoal processState = ProcessAllSubgoals();

            // If everything is done -> activate again
            if (processState == Statusgoal.completed)
                State = Statusgoal.completed;
            return State;
        }
    }
}
