using System;

namespace Fuzzy.Sets
{
    public class LeftSlopeSet : FuzzySet
    {
        private readonly double peakPoint;
        private readonly double leftOffset;

        public LeftSlopeSet(double peakPoint, double leftOffset) : base(peakPoint)
        {
            if (leftOffset < 0) throw new ArgumentOutOfRangeException(nameof(leftOffset));

            this.peakPoint = peakPoint;
            this.leftOffset = leftOffset;
        }

        public override double CalculateMembership(double value = 0)
        {
            if (value > this.peakPoint) { return 1.0; }

            return LeftSlopeMembership(value, this.leftOffset, this.peakPoint);
        }
    }
}