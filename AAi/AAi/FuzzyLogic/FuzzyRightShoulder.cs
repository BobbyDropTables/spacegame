using System;
using System.Collections.Generic;
using System.Text;

namespace AAI.FuzzyLogic
{
    class FuzzyRightShoulder : IFuzzySet
    {
        private double _peakPoint;
        private double _leftOffset;
        private double _rightOffset;

        public FuzzyRightShoulder(double peak, double left, double right) : base(((peak + right) + peak) / 2)
        {
            _peakPoint = peak;
            _leftOffset = left;
            _rightOffset = right;
        }

        public override double CalculateDOM(double val)
        {
            // Check for zero
            if (_leftOffset.Equals(0.0) && val.Equals(_peakPoint))
                return 1.0;

            // Find DOM if left of center
            if ((val <= _peakPoint) && (val > (_peakPoint - _leftOffset)))
            {
                double grad = 1.0 / _leftOffset;

                return grad * (val - (_peakPoint - _leftOffset));
            }

            // Find DOM if right of center
            if (val > _peakPoint)
                return 1.0;

            // Out of range
            return 0.0;
        }
    }
}
