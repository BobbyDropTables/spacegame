using System;
using System.Collections.Generic;
using System.Text;

namespace AAI.FuzzyLogic
{
    class FzAND : IFuzzyTerm
    {
        private readonly List<IFuzzyTerm> _terms;

        public FzAND(IFuzzyTerm first, IFuzzyTerm second)
        {
            _terms = new List<IFuzzyTerm>();

            _terms.Add(first);
            _terms.Add(second);
        }

        public FzAND(IFuzzyTerm first, IFuzzyTerm second, IFuzzyTerm third)
        {
            _terms = new List<IFuzzyTerm>();

            _terms.Add(first);
            _terms.Add(second);
            _terms.Add(third);
        }
        public FzAND(IFuzzyTerm first, IFuzzyTerm second, IFuzzyTerm third, IFuzzyTerm fourth)
        {
            _terms = new List<IFuzzyTerm>();

            _terms.Add(first);
            _terms.Add(second);
            _terms.Add(third);
            _terms.Add(fourth);
        }
        public FzAND(List<IFuzzyTerm> terms)
        {
            _terms = terms;
        }
        public IFuzzyTerm Clone()
        {
            return new FzAND(_terms);
        }

        public double GetDOM()
        {
            double smallest = Double.MaxValue;

            foreach (var term in _terms)
            {
                if (term.GetDOM() < smallest)
                {
                    smallest = term.GetDOM();
                }
            }

            return smallest;
        }

        public void ClearDOM()
        {
            foreach (var term in _terms)
            {
                term.ClearDOM();
            }
        }

        public void ORwithDOM(double val)
        {
            foreach (var term in _terms)
            {
                term.ORwithDOM(val);
            }
        }
    }
}
