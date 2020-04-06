using System;
using System.Collections.Generic;
using System.Text;

namespace AAI.FuzzyLogic
{
    class FzSet : IFuzzyTerm
    {
        private readonly IFuzzySet _set;

        public FzSet(IFuzzySet fs)
        {
            _set = fs;
        }

        public IFuzzyTerm Clone()
        {
            return new FzSet(this._set);
        }

        public double GetDOM()
        {
            return _set.GetDOM();
        }

        public void ClearDOM()
        {
            _set.ClearDOM();
        }

        public void ORwithDOM(double val)
        {
            _set.ORwithDOM(val);
        }
    }
}
