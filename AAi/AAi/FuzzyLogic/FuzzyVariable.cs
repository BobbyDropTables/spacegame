using System;
using System.Collections.Generic;
using System.Text;

namespace AAI.FuzzyLogic
{
    class FuzzyVariable
    {
        private Dictionary<string, IFuzzySet> _memberSets;

        private double _minRange;
        private double _maxRange;

        public FuzzyVariable()
        {
            _memberSets = new Dictionary<string, IFuzzySet>();
            _minRange = 0.0;
            _maxRange = 0.0;
        }

        //this method is called with the upper and lower bound of a set each time a
        ////new set is added to adjust the upper and lower range values accordingly
        private void AdjustRangeToFit(double min, double max)
        {
            if (min < _minRange)
                _minRange = min;
            if (max > _maxRange)
                _maxRange = max;
        }

        //the following methods create instances of the sets named in the method
        //name and adds them to the member set map. Each time a set of any type is
        //added the m_dMinRange and m_dMaxRange are adjusted accordingly. All of the
        //methods return a proxy class representing the newly created instance. This
        //proxy set can be used as an operand when creating the rule base.
        public FzSet AddLeftShoulderSet(string name, double minBound, double peak, double maxBound)
        {
            _memberSets[name] = new FuzzyLeftShoulder(peak, peak - minBound, maxBound - peak);

            // Adjust range
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(_memberSets[name]);
        }
        public FzSet AddRightShoulderSet(string name, double minBound, double peak, double maxBound)
        {
            _memberSets[name] = new FuzzyRightShoulder(peak, peak - minBound, maxBound - peak);

            // Adjust range
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(_memberSets[name]);
        }
        public FzSet AddTriangularSet(string name, double minBound, double peak, double maxBound)
        {
            _memberSets[name] = new FuzzyTriangle(peak, peak - minBound, maxBound - peak);

            // Adjust range
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(_memberSets[name]);
        }
        // public FzSet AddSingletonSet(string name, double minBound, double peak, double maxBound)
        // {
        //     throw new NotImplementedException();
        // }

        //fuzzify a value by calculating its DOM in each of this variable's subsets
        public void Fuzzify(double val)
        {
            if( !((val >= _minRange) && (val <= _maxRange)) )
                throw new Exception("FuzzyVariable: input value is out of bounds.");

            //for each set in the flv calculate the DOM for the given value
            foreach (var member in _memberSets)
            {
                member.Value.SetDOM(member.Value.CalculateDOM(val));
            }
        }
        //defuzzify the variable using the MaxAv method
        public double DeFuzzifyMaxAv()
        {
            double bottom = 0.0;
            double top = 0.0;

            foreach (var member in _memberSets)
            {
                bottom += member.Value.GetDOM();

                top += member.Value.GetRepresentativeVal() * member.Value.GetDOM();
            }

            // If bottom is equal to zero, return 0
            if (bottom.Equals(0))
                return 0.0;

            return top / bottom;
        }

        //defuzzify the variable using the centroid method
        public double DeFuzzifyCentroid(int NumSamples)
        {
            //calculate the step size
            double stepSize = (_maxRange - _minRange) / (double)NumSamples;

            double totalArea = 0.0;
            double sumOfMoments = 0.0;

            for (int samp = 1; samp <= NumSamples; ++samp)
            {
                //for each set get the contribution to the area. This is the lower of the 
                //value returned from CalculateDOM or the actual DOM of the fuzzified 
                //value itself   
                foreach (var member in _memberSets)
                {
                    double contribution = Math.Min(
                        member.Value.CalculateDOM(_minRange + samp * stepSize), 
                        member.Value.GetDOM()
                        );

                    totalArea += contribution;

                    sumOfMoments += (_minRange + samp * stepSize) * contribution;
                }
            }

            // Check if total area equals zero
            if (totalArea.Equals(0))
                return 0.0;

            return (sumOfMoments / totalArea);
        }

    }
}
