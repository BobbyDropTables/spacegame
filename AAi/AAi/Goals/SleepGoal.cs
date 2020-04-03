using System;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using Microsoft.Xna.Framework;

namespace AAI.Goals
{
    internal class SleepGoal : CompositeGoal
    {
        private          int    i;
        private readonly Target Target;

        public SleepGoal(SmartEntity smartEntity, Target target)
        {
            i       = 0;
            State   = Statusgoal.inactive;
            this.smartEntity = smartEntity;
            Target  = target;
            Name    = "Sleep";
        }

        public override void Activate()
        {
            i     = 0;
            State = Statusgoal.active;
        }

        public override Statusgoal Process()
        {
            if (State == Statusgoal.inactive)
                Activate();
            //check if Robot is near food
            if (Math.Abs(smartEntity.Pos.X - Target.Pos.X) < 2)
                if (Math.Abs(smartEntity.Pos.Y - Target.Pos.Y) < 2)
                {
                    smartEntity.Velocity = new Vector2(0, 0);

                    //count how many times this is reached
                    i++;
                    //is it less then 1000 continue
                    if (i > 1000)
                    {
                        smartEntity.tiredness = 0;

                        State = Statusgoal.completed;
                    }
                }

            return State;
        }
    }
}