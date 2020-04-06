using System;
using System.Collections.Generic;
using System.Text;

namespace AAI.FuzzyLogic
{
    class FzOR : IFuzzyTerm
    {
        private readonly List<IFuzzyTerm> _terms = new List<IFuzzyTerm>();

        public FzOR(IFuzzyTerm first, IFuzzyTerm second)
        {
            _terms.Add(first);
            _terms.Add(second);
        }
        public FzOR(IFuzzyTerm first, IFuzzyTerm second, IFuzzyTerm third)
        {
            _terms.Add(first);
            _terms.Add(second);
            _terms.Add(third);
        }
        public FzOR(IFuzzyTerm first, IFuzzyTerm second, IFuzzyTerm third, IFuzzyTerm fourth)
        {
            _terms.Add(first);
            _terms.Add(second);
            _terms.Add(third);
            _terms.Add(fourth);
        }

        public FzOR(List<IFuzzyTerm> list)
        {
            _terms = list;
        }

        public IFuzzyTerm Clone()
        {
            return new FzOR(_terms);
        }

        public double GetDOM()
        {
            double largest = double.MinValue;

            foreach (var term in _terms)
            {
                if (term.GetDOM() > largest)
                    largest = term.GetDOM();
            }

            return largest;
        }

        public void ClearDOM()
        {
            throw new Exception("FzOR: ClearDOM: Invalid context.");
        }

        public void ORwithDOM(double val)
        {
            throw new Exception("FzOR: ORwithDOM: Invalid context.");
        }
    }
}
