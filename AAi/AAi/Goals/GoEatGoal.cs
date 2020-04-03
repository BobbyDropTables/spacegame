using System.Collections.Generic;
using AAI.behaviour;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;

namespace AAI.Goals
{
    class GoEatGoal : CompositeGoal
    {
        private readonly Target        Target;
        public GoEatGoal(Vehicle vehicle, Target target)
        {
            State = Statusgoal.inactive;
            Vehicle       = vehicle;
            Name          = "GoEat";
            Target        = target;
            SubGoals = new List<CompositeGoal>();
        }

        public override void Activate()
        {
            State = Statusgoal.active;
            SubGoals.Clear();
            SubGoals.Add(new PathFollowingGoal(Vehicle,Target));
            SubGoals.Add(new EatGoal(Vehicle,Target));
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