using System;

namespace Fuzzy.Sets
{
    public abstract class FuzzySet
    {
        private double degreeOfMembership;

        /// <summary>
        /// Degree of membership of a set. Value from 0.0 to 1.0
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">When value is less than 0 or more than 1</exception>
        public double DegreeOfMembership
        {
            get => this.degreeOfMembership;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                if (value > 1) throw new ArgumentOutOfRangeException(nameof(value));

                this.degreeOfMembership = value;
            }
        }

        public double RepresentativeValue { get; protected set; }

        public FuzzySet(double representativeValue)
        {
            this.RepresentativeValue = representativeValue;
        }

        public abstract double CalculateMembership(double value = 0);

        public void OrWithMembership(double val)
        {
            if (this.DegreeOfMembership < val) { this.DegreeOfMembership = val; }
        }

        public void ClearMembership()
        {
            this.DegreeOfMembership = 0.0;
        }

        /// <summary>
        /// Calculates degree of membership for a left slope
        /// </summary>
        /// <param name="value">Value to calculate membership of</param>
        /// <param name="leftOffset">Left-offset from peak point</param>
        /// <param name="peakPoint">Point where membership is 1.0</param>
        /// <returns>Degree of membership: value between 0 and 1.0</returns>
        public static double LeftSlopeMembership(double value, double leftOffset, double peakPoint)
        {
            if (value > peakPoint) return 0.0;
            if (value < peakPoint - leftOffset) return 0.0;

            double distanceToLeftOffset = value - (peakPoint - leftOffset);
            // Upwards slope
            double slope = 1.0 / leftOffset;

            return slope * distanceToLeftOffset;
        }

        /// <summary>
        /// Calculates degree of membership for a right slope
        /// </summary>
        /// <param name="value">Value to calculate membership of</param>
        /// <param name="rightOffset">Right-offset from peak point</param>
        /// <param name="peakPoint">Point where membership is 1.0</param>
        /// <returns>Degree of membership: value between 0 and 1.0</returns>
        public static double RightSlopeMembership(double value, double rightOffset, double peakPoint)
        {
            if (value < peakPoint) return 0.0;
            if (value > peakPoint + rightOffset) return 0.0;

            double distanceToPeakPoint = value - peakPoint;
            // Downwards slope
            double slope = 1.0 / -rightOffset;

            return (slope * distanceToPeakPoint) + 1.0;
        }
    }
}