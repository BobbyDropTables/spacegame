using System;
using System.Collections.Generic;
using System.Text;

namespace AAI.FuzzyLogic
{
    class FuzzyTriangle : IFuzzySet
    {
        private double _peakPoint;
        private double _leftOffset;
        private double _rightOffset;
        public FuzzyTriangle(double mid, double left, double right) : base(mid)
        {
            _peakPoint = mid;
            _leftOffset = left;
            _rightOffset = right;
        }

        public override double CalculateDOM(double val)
        {
            // Prevent divide by zero
            if ((_rightOffset.Equals(0.0) && _peakPoint.Equals(val)) ||
                (_leftOffset.Equals(0.0) && _peakPoint.Equals(val)))
                return 1.0;

            // Find DOM if left of center
            if ((val <= _peakPoint) && (val >= (_peakPoint - _leftOffset)))
            {
                double grad = 1.0 / _leftOffset;

                return grad * (val - (_peakPoint - _leftOffset));
            }

            // Find DOM if right of center
            if ((val > _peakPoint) && val < (_peakPoint + _rightOffset))
            {
                double grad = 1.0 / -_rightOffset;

                return grad * (val - _peakPoint) + 1.0;
            }

            // Out of range
            return 0.0;
        }
    }
}
