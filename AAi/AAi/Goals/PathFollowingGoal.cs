using System.Collections.Generic;
using AAI.behaviour;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;

namespace AAI.Goals
{
    internal class PathFollowingGoal : CompositeGoal
    {
        private          int           i;
        private  PathFollowing PathFollowing;
        private readonly Target        Target;

        public PathFollowingGoal(SmartEntity smartEntity, Target target)
        {
            i             = 0;
            State = Statusgoal.inactive;
            this.smartEntity = smartEntity;
            
            Name          = "Pathfollowing";
            Target        = target;
        }

        public override void Activate()
        {
            State = Statusgoal.active;
            var Path = smartEntity.MyWorld.gameMap.PathingPipeline(smartEntity.Pos, Target.Pos, smartEntity.MyWorld.walls);
            PathFollowing = new PathFollowing(smartEntity , Path);
            smartEntity.Behaviours = new List<SteeringBehaviour>
            {
                PathFollowing,
                new WallAvoidance(smartEntity,15),
            };
        }

        public override Statusgoal Process()
        {
            if (State == Statusgoal.inactive)
                Activate();
            //check if Path is complete
            if (PathFollowing.Finished)
                State = Statusgoal.completed;

            return State;
        }
    }
}