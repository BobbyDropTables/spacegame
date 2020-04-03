using System.Collections.Generic;

namespace AAI.Goals
{
    public abstract class CompositeGoal : BaseGoal
    {
        public List<CompositeGoal> SubGoals;

        public void RemoveAllSubGoals()
        {
            if (SubGoals == null)
                return;
            for (int i = 0; i < SubGoals.Count; i++)
            {
                DeActivateGoals(SubGoals[i]);
            }
            SubGoals = new List<CompositeGoal>();
        }

        public void DeActivateGoals(CompositeGoal sub)
        {
            sub.State = Statusgoal.inactive;
            if (sub.SubGoals == null)
                return;
            for (int i = 0; i < sub.SubGoals.Count; i++)
            {
                DeActivateGoals(sub.SubGoals[i]);
            }
        }

        public Statusgoal ProcessAllSubgoals()
        {
            if (SubGoals.Count <= 0)
                return Statusgoal.completed;

            foreach (var g in SubGoals)
            {
                switch (g.State)
                {
                    case Statusgoal.completed:
                        continue;
                    case Statusgoal.active:
                        g.Process();
                        return Statusgoal.active;
                    case Statusgoal.inactive:
                        g.Process();
                        return Statusgoal.inactive;
                }
            }
            return Statusgoal.completed;
        }
    }
}