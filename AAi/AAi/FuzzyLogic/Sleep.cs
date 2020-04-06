using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI.FuzzyLogic
{
    class Sleep : Goal
    {
        private const double _MAX_SLEEP = 40;
        public Sleep() : base()
        {
            _module = new FuzzyModule();

            // Antecedents
            FuzzyVariable Sleep = _module.CreateFLV("Sleep");
            var Sleep_Low = Sleep.AddLeftShoulderSet("Sleep_Low", 0, 10, 20);
            var Sleep_Med = Sleep.AddTriangularSet("Sleep_Med", 10, 20, 30);
            var Sleep_High = Sleep.AddRightShoulderSet("Sleep_High", 20, 30, _MAX_SLEEP);

            FuzzyVariable Distance = _module.CreateFLV("Distance");
            var Distance_Close = Distance.AddLeftShoulderSet("Distance_Close", 0, 40, 80);
            var Distance_Medium = Distance.AddTriangularSet("Distance_Medium", 40, 80, 400);
            var Distance_Far = Distance.AddRightShoulderSet("Distance_Far", 80, 400, MAX_DISTANCE);

            // Consequence
            FuzzyVariable Desirability = _module.CreateFLV("Desirability");
            var Undesirable = Desirability.AddLeftShoulderSet("Undesirable", 0, 25, 50);
            var Desirable = Desirability.AddTriangularSet("Desirable", 25, 50, 75);
            var VeryDesirable = Desirability.AddRightShoulderSet("VeryDesirable", 50, 75, 100);

            // Rules
            _module.AddRule(new FzAND(Sleep_Low, Distance_Close), Undesirable);
            _module.AddRule(new FzAND(Sleep_Low, Distance_Medium), Undesirable);
            _module.AddRule(new FzAND(Sleep_Low, Distance_Far), Undesirable);
            _module.AddRule(new FzAND(Sleep_Med, Distance_Close), Desirable);
            _module.AddRule(new FzAND(Sleep_Med, Distance_Medium), Desirable);
            _module.AddRule(new FzAND(Sleep_Med, Distance_Far), Undesirable);
            _module.AddRule(new FzAND(Sleep_High, Distance_Close), VeryDesirable);
            _module.AddRule(new FzAND(Sleep_High, Distance_Medium), VeryDesirable);
            _module.AddRule(new FzAND(Sleep_High, Distance_Far), Desirable);
        }

        public override double CalculateDesirability(double distance, double value)
        {
            _module.Fuzzify("Distance", LimitDistance(distance));
            _module.Fuzzify("Sleep", LimitValue(value));

            return _module.DeFuzzify("Desirability", FuzzyModule.DefuzzifyMethod.max_av);
        }

        protected override double LimitValue(double value)
        {
            double newVal = value;

            if (newVal > _MAX_SLEEP)
                newVal = _MAX_SLEEP;
            if (newVal < 0)
                newVal = 0;

            return newVal;
        }
    }
}
