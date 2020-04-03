using System.Collections.Generic;
using AAI.behaviour;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;

namespace AAI.Goals
{
    internal class PathFollowingGoal : CompositeGoal
    {
        private          int           i;
        private readonly PathFollowing PathFollowing;
        private readonly Target        Target;

        public PathFollowingGoal(Vehicle vehicle, Target target)
        {
            i             = 0;
            State = Statusgoal.inactive;
            Vehicle       = vehicle;
            PathFollowing = new PathFollowing(Vehicle);
            Name          = "Pathfollowing";
            Target        = target;
        }

        public override void Activate()
        {
            State = Statusgoal.active;
            Vehicle.Path = Vehicle.MyWorld.gameMap.PathingPipeline(Vehicle.Pos, Target.Pos, Vehicle.MyWorld.walls);
            Vehicle.Behaviours = new List<SteeringBehaviour>
            {
                PathFollowing,
                new WallAvoidance(Vehicle, 15)
            };
        }

        public override Statusgoal Process()
        {
            if (State == Statusgoal.inactive)
                Activate();
            //check if Path is complete
            if (PathFollowing.finished)
                State = Statusgoal.completed;

            return State;
        }
    }
}