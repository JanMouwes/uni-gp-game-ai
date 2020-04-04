using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fuzzy.Sets;

namespace Fuzzy
{
    public class Variable
    {
        private Dictionary<string, FuzzySet> categories;
        private double minRange;
        private double maxRange;

        public Variable()
        {
            this.categories = new Dictionary<string, FuzzySet>();
        }

        public void AddTriangleSet(string name, double minBound, double peakPoint, double maxBound)
        {
            this.categories.Add(name, new TriangleSet(peakPoint, peakPoint - minBound, maxBound - peakPoint));
            AdjustRange(minBound, maxBound);
        }

        public void AddLeftSlopeSet(string name, double minBound, double peakPoint, double maxBound)
        {
            this.categories.Add(name, new LeftSlopeSet(peakPoint, peakPoint - minBound));
            AdjustRange(minBound, maxBound);
        }

        public void AddRightSlopeSet(string name, double minBound, double peakPoint, double maxBound)
        {
            this.categories.Add(name, new RightSlopeSet(peakPoint, maxBound - peakPoint));
            AdjustRange(minBound, maxBound);
        }

        public void Fuzzify(double value)
        {
            foreach (FuzzySet set in this.categories.Values) { set.DegreeOfMembership = set.CalculateMembership(value); }
        }

        public double DefuzzifyMaxAverage()
        {
            IEnumerable<FuzzySet> validCategories = this.categories.Values.Where(category => category.DegreeOfMembership > 0);
            
            foreach (FuzzySet category in validCategories)
            {
                category.DegreeOfMembership;
            }
        }

        public double DefuzzifyCentroid(int sampleCount);

        private void AdjustRange(double newMinRange, double newMaxRange)
        {
            if (newMinRange < this.minRange) { this.minRange = newMinRange; }

            if (newMaxRange > this.maxRange) { this.maxRange = newMaxRange; }
        }
    }
}