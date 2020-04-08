using System;
using System.Collections.Generic;
using Fuzzy.Rules;
using Fuzzy.Terms;

namespace Fuzzy
{
    public enum DefuzzifyMethods
    {
        Centroid,
        MaxAverage
    }

    public class FuzzyModule
    {
        private readonly Dictionary<string, Variable> variables;
        private ICollection<Rule> rules;

        public FuzzyModule()
        {
            this.variables = new Dictionary<string, Variable>();
            this.rules = new LinkedList<Rule>();
        }

        /// <summary>
        /// Create fuzzy variable and store it
        /// </summary>
        /// <param name="name">Unique identifier</param>
        /// <returns>Variable</returns>
        /// <exception cref="Exception">When variable with that name already exists</exception>
        public Variable CreateVariable(string name)
        {
            if (this.variables.ContainsKey(name)) { throw new Exception($"Fuzzy variable with name '{name}' already exists"); }

            Variable variable = new Variable();

            this.variables.Add(name, variable);

            return variable;
        }

        public void AddRule(Term antecedent, Term consequence)
        {
            Rule rule = new Rule(antecedent, consequence);
            this.rules.Add(rule);
        }

        public void Fuzzify(string variableName, double value)
        {
            if (!this.variables.ContainsKey(variableName)) { throw new Exception($"Fuzzy variable with name '{variableName}' does not exist"); }

            Variable variable = this.variables[variableName];

            variable.Fuzzify(value);
        }

        public double Defuzzify(string variableName, DefuzzifyMethods method)
        {
            if (!this.variables.TryGetValue(variableName, out Variable variable)) { throw new ArgumentException($"Variable {variableName} does not exist"); }

            foreach (Rule rule in this.rules)
            {
                rule.ClearConsequentConfidence();
                rule.Calculate();
            }

            switch (method)
            {
                case DefuzzifyMethods.Centroid:
                    return variable.DefuzzifyCentroid(10);
                case DefuzzifyMethods.MaxAverage:
                    return variable.DefuzzifyMaxAverage();
                default:
                    // Shouldn't ever happen
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }
    }
}