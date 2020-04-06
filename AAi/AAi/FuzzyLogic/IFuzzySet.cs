using System;
using System.Collections.Generic;
using System.Text;

namespace AAI.FuzzyLogic
{
    abstract class IFuzzySet
    {
        protected double _degreeOfMembership;

        // Maximum of the set's membership function.
        protected double _representativeValue;


        protected IFuzzySet(double representativeValue)
        {
            this._degreeOfMembership = 0.0;
            this._representativeValue = representativeValue;
        }

        public abstract double CalculateDOM(double val);

        public void ORwithDOM(double val)
        {
            if (val > _degreeOfMembership)
                _degreeOfMembership = val;
        }
        public double GetDOM()
        {
            return _degreeOfMembership;
        }

        public void SetDOM(double val)
        {
            if(val <= 1 && val >= 0)
                _degreeOfMembership = val;
            else throw new Exception("IFuzzySet: Invalid value");
        }

        public void ClearDOM()
        {
            this._degreeOfMembership = 0.0;
        }

        public double GetRepresentativeVal()
        {
            return _representativeValue;
        }

        public void SetRepresentativeVal(double val)
        {
            _representativeValue = val;
        }
    }
}
