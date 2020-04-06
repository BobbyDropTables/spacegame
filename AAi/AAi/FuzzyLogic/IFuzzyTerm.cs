using System;
using System.Collections.Generic;
using System.Text;

namespace AAI.FuzzyLogic
{
    interface IFuzzyTerm
    {

        IFuzzyTerm Clone();
        double GetDOM();
        void ClearDOM();
        void ORwithDOM(double val);
    }
}
