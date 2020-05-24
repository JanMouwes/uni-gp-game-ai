using System.Collections.Generic;
using System.Linq;

namespace Fuzzy.Terms
{
    public class And : Term
    {
        private readonly ICollection<Term> terms;

        public And(Term termA, Term termB)
        {
            this.terms = new[] {termA, termB};
        }

        public And(IEnumerable<Term> terms)
        {
            this.terms = new List<Term>(terms);
        }

        public override double Membership => this.terms.Select(term => term.Membership).Min();

        public override void ClearMembership()
        {
            foreach (Term term in this.terms) { term.ClearMembership(); }
        }

        public override void OrWithMembership(double value)
        {
            foreach (Term term in this.terms) { term.OrWithMembership(value); }
        }
    }
}