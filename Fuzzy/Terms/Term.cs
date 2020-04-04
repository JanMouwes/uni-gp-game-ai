namespace Fuzzy.Terms
{
    // TODO make this a struct?
    public abstract class Term
    {
        public abstract double Membership { get; }

        public abstract void ClearMembership();

        public abstract void OrWithMembership(double value);
    }
}