using Fuzzy.Terms;

namespace Fuzzy.Rules
{
    public class Rule
    {
        /// <summary>
        /// Condition
        /// </summary>
        private readonly Term antecedent;

        /// <summary>
        /// Result
        /// </summary>
        private readonly Term consequence;


        public Rule(Term antecedent, Term consequence)
        {
            this.antecedent = antecedent;
            this.consequence = consequence;
        }

        public void ClearConsequentConfidence()
        {
            this.consequence.ClearMembership();
        }

        public void Calculate()
        {
            this.consequence.OrWithMembership(this.antecedent.Membership);
        }
    }
}