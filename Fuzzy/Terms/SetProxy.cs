using Fuzzy.Sets;

namespace Fuzzy.Terms
{
    /// <summary>
    /// Proxy for FuzzySet
    /// </summary>
    public class SetProxy : Term
    {
        private readonly FuzzySet innerSet;

        public SetProxy(FuzzySet innerSet)
        {
            this.innerSet = innerSet;
        }

        public override double Membership => this.innerSet.DegreeOfMembership;

        public override void ClearMembership() => this.innerSet.ClearMembership();

        public override void OrWithMembership(double value) => this.innerSet.OrWithMembership(value);
    }
}