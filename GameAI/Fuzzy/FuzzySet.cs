using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Fuzzy
{
    public abstract class FuzzySet
    {
        public double m_dDOM { get; set; }
        protected double m_dRepresentativeValue;

        public FuzzySet(double RepVal)
        {
            m_dRepresentativeValue = RepVal;
        }

        public double CalculateDOM(double val = 0)
        {
            return val * m_dDOM;
        }

        public void ORwithDOM(double val)
        {
            if (m_dDOM < val)
            {
                m_dDOM = val;
            }
        }

        public double GetRepresentativeVal()
        {
            return m_dRepresentativeValue;
        }

        public void ClearDOM()
        {
            m_dDOM = 0.0;
        }
    }
}
