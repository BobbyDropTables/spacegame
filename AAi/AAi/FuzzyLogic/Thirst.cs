using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAI.FuzzyLogic;

namespace AAI.Goals
{
    class Thirst : Goal
    {
        private const double _MAX_THIRST = 40;
        public Thirst() : base()
        {
            _module = new FuzzyModule();

            // Antecedents
            FuzzyVariable Thirst = _module.CreateFLV("Thirst");
            var Thirst_Low = Thirst.AddLeftShoulderSet("Thirst_Low", 0, 10, 20);
            var Thirst_Med = Thirst.AddTriangularSet("Thirst_Med", 10, 20, 30);
            var Thirst_High = Thirst.AddRightShoulderSet("Thirst_High", 20, 30, _MAX_THIRST);

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
            _module.AddRule(new FzAND(Thirst_Low, Distance_Close), Undesirable);
            _module.AddRule(new FzAND(Thirst_Low, Distance_Medium), Undesirable);
            _module.AddRule(new FzAND(Thirst_Low, Distance_Far), Undesirable);
            _module.AddRule(new FzAND(Thirst_Med, Distance_Close), Desirable);
            _module.AddRule(new FzAND(Thirst_Med, Distance_Medium), Undesirable);
            _module.AddRule(new FzAND(Thirst_Med, Distance_Far), Undesirable);
            _module.AddRule(new FzAND(Thirst_High, Distance_Close), VeryDesirable);
            _module.AddRule(new FzAND(Thirst_High, Distance_Medium), VeryDesirable);
            _module.AddRule(new FzAND(Thirst_High, Distance_Far), Desirable);

        }
        public override double CalculateDesirability(double distance, double value)
        {
            _module.Fuzzify("Distance", LimitDistance(distance));
            _module.Fuzzify("Thirst", LimitValue(value));

            return _module.DeFuzzify("Desirability", FuzzyModule.DefuzzifyMethod.max_av);
        }

        protected override double LimitValue(double value)
        {
            double newVal = value;

            if (newVal > _MAX_THIRST)
                newVal = _MAX_THIRST;
            if (newVal < 0)
                newVal = 0;

            return newVal;
        }
    }
}
