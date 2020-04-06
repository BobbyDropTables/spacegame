using System;
using System.Collections.Generic;
using System.Text;

namespace AAI.FuzzyLogic
{
    class FuzzyLeftShoulder : IFuzzySet
    {
        private double _peakPoint;
        private double _leftOffset;
        private double _rightOffset;

        public FuzzyLeftShoulder(double peak, double left, double right) : base(((peak - left) + peak) / 2)
        {
            _peakPoint = peak;
            _leftOffset = left;
            _rightOffset = right;
        }
        public override double CalculateDOM(double val)
        {
            // Check for zero
            if ((_rightOffset.Equals(0.0) && _peakPoint.Equals(val)) ||
                (_leftOffset.Equals(0.0) && _peakPoint.Equals(val)))
                return 1.0;

            // Find DOM if right of center
            if ((val >= _peakPoint) && (val < (_peakPoint + _rightOffset)))
            {
                double grad = 1.0 / -_rightOffset;

                return grad * (val - _peakPoint) + 1.0;
            }

            // Find DOM if left of center
            if ((val < _peakPoint) && (val >= _peakPoint - _leftOffset))
            {
                return 1.0;
            }

            // Out of range
            return 0.0;
        }
    }
}
