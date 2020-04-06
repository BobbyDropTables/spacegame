using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI.FuzzyLogic
{
    class Hunger : Goal
    {
        private const double _MAX_HUNGER = 60;
        public Hunger() : base()
        {
            _module = new FuzzyModule();

            // Antecedents
            FuzzyVariable Hunger = _module.CreateFLV("Hunger");
            var Hunger_Low = Hunger.AddLeftShoulderSet("Hunger_Low", 0, 15, 30);
            var Hunger_Med = Hunger.AddTriangularSet("Hunger_Med", 15, 30, 45);
            var Hunger_Big = Hunger.AddRightShoulderSet("Hunger_Big", 30, 45, _MAX_HUNGER);

            FuzzyVariable Distance = _module.CreateFLV("Distance");
            var Distance_Close = Distance.AddLeftShoulderSet("Distance_Close", 0, 80, 160);
            var Distance_Medium = Distance.AddTriangularSet("Distance_Medium", 80, 240, 400);
            var Distance_Far = Distance.AddRightShoulderSet("Distance_Far", 240, 400, MAX_DISTANCE);

            // Consequence
            FuzzyVariable Desirability = _module.CreateFLV("Desirability");
            var Undesirable = Desirability.AddLeftShoulderSet("Undesirable", 0, 25, 50);
            var Desirable = Desirability.AddTriangularSet("Desirable", 25, 50, 75);
            var VeryDesirable = Desirability.AddRightShoulderSet("VeryDesirable", 50, 75, 100);

            // Rules
            _module.AddRule(new FzAND(Hunger_Low, Distance_Close), Undesirable);
            _module.AddRule(new FzAND(Hunger_Low, Distance_Medium), Undesirable);
            _module.AddRule(new FzAND(Hunger_Low, Distance_Far), Undesirable);
            _module.AddRule(new FzAND(Hunger_Med, Distance_Close), Desirable);
            _module.AddRule(new FzAND(Hunger_Med, Distance_Medium), Undesirable);
            _module.AddRule(new FzAND(Hunger_Med, Distance_Far), Undesirable);
            _module.AddRule(new FzAND(Hunger_Big, Distance_Close), VeryDesirable);
            _module.AddRule(new FzAND(Hunger_Big, Distance_Medium), VeryDesirable);
            _module.AddRule(new FzAND(Hunger_Big, Distance_Far), Desirable);

        }
        public override double CalculateDesirability(double distance, double value)
        {
            _module.Fuzzify("Distance", LimitDistance(distance));
            _module.Fuzzify("Hunger", LimitValue(value));

            return _module.DeFuzzify("Desirability", FuzzyModule.DefuzzifyMethod.max_av);
        }

        protected override double LimitValue(double value)
        {
            double newVal = value;

            if (newVal > _MAX_HUNGER)
                newVal = _MAX_HUNGER;
            if (newVal < 0)
                newVal = 0;

            return newVal;
        }

    }
}
