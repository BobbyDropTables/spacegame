using System;
using System.Collections.Generic;
using System.Text;

namespace AAI.FuzzyLogic
{
    class FuzzyRule
    {
        private IFuzzyTerm _antecedent;
        private IFuzzyTerm _consequent;

        public FuzzyRule(IFuzzyTerm antecedent, IFuzzyTerm consequence)
        {
            _antecedent = antecedent.Clone();
            _consequent = consequence.Clone();
        }

        public void SetConfidenceOfConsequentToZero() { _consequent.ClearDOM(); }

        // Update the DOM (the confidence) of the consequent term with the DOM of the antecedent term.
        public void Calculate()
        {
            _consequent.ORwithDOM(_antecedent.GetDOM());
        }
    }
}
