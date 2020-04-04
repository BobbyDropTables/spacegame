using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;

namespace AAI.Goals
{
    class GoSleepGoal : CompositeGoal
    {
        private Target Target;
        public GoSleepGoal(SmartEntity smartEntity, Target target)
        {
            State    = Statusgoal.inactive;
            this.smartEntity  = smartEntity;
            Name     = "Go Sleep";
            Target   = target;
            SubGoals = new List<CompositeGoal>();
        }

        public override void Activate()
        {
            State = Statusgoal.active;
            SubGoals.Clear();
            SubGoals.Add(new PathFollowingGoal(smartEntity, Target));
            SubGoals.Add(new SleepGoal(smartEntity, Target));
        }

        public override Statusgoal Process()
        {
            if (State == Statusgoal.inactive)
            {
                Activate();
            }

            Statusgoal processState = ProcessAllSubgoals();

            // If everything is done -> mark completed
            if (processState == Statusgoal.completed)
                State = Statusgoal.completed;
            return State;
        }
    }
}
