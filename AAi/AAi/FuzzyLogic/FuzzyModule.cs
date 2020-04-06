using System;
using System.Collections.Generic;
using System.Text;

namespace AAI.FuzzyLogic
{
    class FuzzyModule
    {
        // Dictionary of all the fuzzy variables this module uses
        private Dictionary<string, FuzzyVariable> _variables;

        // List containing all the fuzzy rules
        private List<FuzzyRule> _rules;

        public FuzzyModule()
        {
            _variables = new Dictionary<string, FuzzyVariable>();
            _rules = new List<FuzzyRule>();
        }

        public enum DefuzzifyMethod
        {
            max_av, centroid
        }

        public const int NUM_SAMPLES = 15;

        // Zeros the DOMs of the consequents of each rule. Used by Defuzzify()
        private void _SetConfidencesOfConsequentsToZero()
        {
            foreach (var rule in _rules)
            {
                rule.SetConfidenceOfConsequentToZero();
            }
        }

        // Return a new "empty" fuzzy variable.
        public FuzzyVariable CreateFLV(string name)
        {
            _variables[name] = new FuzzyVariable();
            
            return _variables[name];
        }

        // Adds a rule to the module
        public void AddRule(IFuzzyTerm antecedent, IFuzzyTerm consequence)
        {
            _rules.Add(new FuzzyRule(antecedent, consequence));
        }

        // Calls the Fuzzify method of the named FLV
        public void Fuzzify(string name, double val)
        {
            if(!_variables.ContainsKey(name))
                throw new Exception("FuzzyModule: Fuzzify: Key not found.");

            _variables[name].Fuzzify(val);
        }

        // Given a fuzzy variable and a defuzzification method this returns a crisp value
        public double DeFuzzify(string key, DefuzzifyMethod method)
        {
            if (!_variables.ContainsKey(key))
                throw new Exception("FuzzyModule: DeFuzzify: Key not found.");

            // Clear the DOMs
            _SetConfidencesOfConsequentsToZero();

            // Process
            foreach (var rule in _rules)
            {
                rule.Calculate();
            }

            // Defuzzify using specific method
            switch (method)
            {
                case DefuzzifyMethod.centroid:
                    return _variables[key].DeFuzzifyCentroid(NUM_SAMPLES);

                case DefuzzifyMethod.max_av:
                    return _variables[key].DeFuzzifyMaxAv();
            }

            return 0;
        }
    }
}
