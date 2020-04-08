using System;

namespace Fuzzy.Sets
{
    public class TriangleSet : FuzzySet
    {
        private readonly double peakPoint;
        private readonly double leftOffset;
        private readonly double rightOffset;

        /// <param name="peakPoint">Middle of the triangle</param>
        /// <param name="leftOffset">Distance from middle to the left-most corner of the triangle</param>
        /// <param name="rightOffset">Distance from middle to the right-most corner of the triangle</param>
        /// <exception cref="ArgumentException">When leftOffset or rightOffset lower than 0</exception>
        public TriangleSet(double peakPoint, double leftOffset, double rightOffset) : base(peakPoint)
        {
            if (leftOffset < 0) { throw new ArgumentException(nameof(leftOffset)); }

            if (rightOffset < 0) { throw new ArgumentException(nameof(rightOffset)); }

            this.peakPoint = peakPoint;
            this.leftOffset = leftOffset;
            this.rightOffset = rightOffset;
        }

        public override double CalculateMembership(double value = 0)
        {
            // Value falls outside the range of the triangle
            if (value < this.peakPoint - this.leftOffset || value > this.peakPoint + this.rightOffset) { return 0.0; }

            // Triangle's offsets are both zero 
            if (this.peakPoint.Equals(value) && (this.leftOffset.Equals(0) || this.rightOffset.Equals(0))) { return 1.0; }

            // Value to the left side of peak point
            if (value <= this.peakPoint) { return LeftSlopeMembership(value, this.leftOffset, this.peakPoint); }

            // Value must be greater than peakPoint
            return RightSlopeMembership(value, this.rightOffset, this.peakPoint);
        }
    }
}