using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI.FuzzyLogic
{
    /**
     * ABSTRACT CLASS Goal
     *
     * CLASS TO IMPLEMENT BASIC FUZZY BEHAVIOUR
     */
    abstract class Goal
    {
        protected FuzzyModule _module;
        protected const double MAX_DISTANCE = 1000;

        public abstract double CalculateDesirability(double distance, double value);
        protected abstract double LimitValue(double value);
        protected double LimitDistance(double value)
        {
            double newVal = value;

            if (newVal > MAX_DISTANCE)
                newVal = MAX_DISTANCE;
            if (newVal < 0)
                newVal = 0;

            return newVal;
        }
    }
}
