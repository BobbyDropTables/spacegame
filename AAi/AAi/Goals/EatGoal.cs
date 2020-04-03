using System;
using System.Collections.Generic;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using Microsoft.Xna.Framework;

namespace AAI.Goals
{
    internal class EatGoal : CompositeGoal
    {
        private          int        i;
        private Target Target;

        public EatGoal(Vehicle vehicle, Target target)
        {
            i = 0;
            State = Statusgoal.inactive;
            Vehicle  = vehicle;
            Target = target;
            Name     = "Eating";
        }

        public override void Activate()
        {
            State = Statusgoal.active;
        }

        public override Statusgoal Process()
        {
            if (State == Statusgoal.inactive)
                Activate();
            //check if Vehicle is near food
            if (Math.Abs(Vehicle.Pos.X - Target.Pos.X) < 2)
                if (Math.Abs(Vehicle.Pos.Y - Target.Pos.Y) < 2)
                {
                    Vehicle.Velocity = new Vector2(0,0);
                    //count how many times this is reached
                    i++;
                    //is it less then 10 continue
                    if (i <= 100)
                    {
                        if (i == 100)
                        {
                            if (Vehicle.hunger >= 10)
                            {
                                Vehicle.hunger -= 10;
                            }
                            else
                            {
                                Vehicle.hunger = 0;
                            }
                        }
                        
                    }
                    //if it's higher then 10 the goal is completed
                    else
                    {
                        State = Statusgoal.completed;
                    }
                }

            return State;
        }
    }
}