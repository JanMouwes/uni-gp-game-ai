using System;

namespace Fuzzy.Sets
{
    public class RightSlopeSet : FuzzySet
    {
        private readonly double peakPoint;
        private readonly double rightOffset;

        public RightSlopeSet(double peakPoint, double rightOffset) : base(peakPoint)
        {
            if (rightOffset < 0) throw new ArgumentOutOfRangeException(nameof(rightOffset));

            this.peakPoint = peakPoint;
            this.rightOffset = rightOffset;
        }

        public override double CalculateMembership(double value = 0)
        {
            if (value < this.peakPoint) { return 1.0; }

            return RightSlopeMembership(value, this.rightOffset, this.peakPoint);
        }
    }
}