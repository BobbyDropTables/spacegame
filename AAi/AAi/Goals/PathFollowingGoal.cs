using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using AAI.behaviour;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using Microsoft.Xna.Framework;

namespace AAI.Goals
{
    internal class PathFollowingGoal : CompositeGoal
    {
        private          int           i;
        private  PathFollowing PathFollowing;
        private readonly Target        Target;
        System.Timers.Timer timer;

        public PathFollowingGoal(SmartEntity smartEntity, Target target)
        {
            i             = 0;
            State = Statusgoal.inactive;
            this.smartEntity = smartEntity;
            
            Name          = "Pathfollowing";
            Target        = target;
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OutOfTime);
            timer.Interval = 20000;
        }
        public override void Activate()
        {
            State = Statusgoal.active;
            
            //create a path
            var Path = smartEntity.MyWorld.gameMap.PathingPipeline(smartEntity.Pos, Target.Pos, smartEntity.MyWorld.walls);
            PathFollowing = new PathFollowing(smartEntity , Path);
            //set behaviour
            smartEntity.Behaviours = new List<SteeringBehaviour>
            {
                PathFollowing,
                new WallAvoidance(smartEntity,15),
            };
            timer.Enabled = true;
        }

        private void OutOfTime(object source, ElapsedEventArgs e)
        {
            smartEntity.Pos = smartEntity.MyWorld.RandomVector2inmap();
            Activate();
        }

        public override Statusgoal Process()
        {
            if (State == Statusgoal.inactive)
                Activate();
            //check if Path is complete
            if (PathFollowing.Finished)
            {
                timer.Stop();
                State = Statusgoal.completed;
            }
                

            return State;
        }
    }
}