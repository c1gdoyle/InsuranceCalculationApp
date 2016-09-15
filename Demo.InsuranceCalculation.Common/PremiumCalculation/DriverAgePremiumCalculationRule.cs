using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.InsuranceCalculation.Data;

namespace Demo.InsuranceCalculation.PremiumCalculation
{
    /// <summary>
    /// An implementation of <see cref="IPremiumCalculationRule"/> responsible for calculating
    /// insurance premium based on the age of the drivers on the policy.
    /// </summary>
    public class DriverAgePremiumCalculationRule : IPremiumCalculationRule
    {
        public decimal CalculatePremium(InsurancePolicy policy, decimal currentPremium)
        {
            return currentPremium;
        }
    }
}
