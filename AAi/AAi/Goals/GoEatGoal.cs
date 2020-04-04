using System.Collections.Generic;
using AAI.behaviour;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using Microsoft.Xna.Framework;

namespace AAI.Goals
{
    class GoEatGoal : CompositeGoal
    {
        private Target        Target;
        public GoEatGoal(SmartEntity smartEntity, Target target)
        {
            State = Statusgoal.inactive;
            this.smartEntity = smartEntity;
            Name          = "Go Eat";
            Target        = target;
            SubGoals = new List<CompositeGoal>();
        }

        public override void Activate()
        {
            //Target = new Target(new Vector2(Robot.MyWorld.Random.Next(20, 1260), Robot.MyWorld.Random.Next(20, 940)), Robot.MyWorld);
            State = Statusgoal.active;
            SubGoals.Clear();
            SubGoals.Add(new PathFollowingGoal(smartEntity,Target));
            SubGoals.Add(new EatGoal(smartEntity,Target));
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