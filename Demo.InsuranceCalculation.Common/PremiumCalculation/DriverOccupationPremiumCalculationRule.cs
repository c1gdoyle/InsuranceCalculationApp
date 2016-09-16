using System.Linq;
using Demo.InsuranceCalculation.Data;

namespace Demo.InsuranceCalculation.PremiumCalculation
{
    /// <summary>
    /// An implementation of <see cref="IPremiumCalculationRule"/> responsible for calculating
    /// insurance premium based on the occupation of the drivers on the policy.
    /// </summary>
    public class DriverOccupationPremiumCalculationRule : IPremiumCalculationRule
    {
        /// <summary>
        /// Gets the order in which this rule is to be applied
        /// when calculating the insurance premium.
        /// 
        /// In this case 2.
        /// </summary>
        public int OrderOfPrecedence
        {
            get { return 2; }
        }

        /// <summary>
        /// Calculates the cost of an insurance premium for a given insurance policy.
        /// </summary>
        /// <param name="policy">The insurance policy.</param>
        /// <param name="currentPremium">The current premium amount.</param>
        /// <returns>
        /// The cost of the insurance premium after assessing the policy against 
        /// this rule.
        /// </returns>
        /// <remarks>
        /// - If there is a driver who is a Chauffeur on the policy increase the premium by
        /// 10 %.
        /// - If there is a driver who is an Accountant on the policy decrease the premium by
        /// 10 %.
        /// </remarks>
        public decimal CalculatePremium(InsurancePolicy policy, decimal currentPremium)
        {
            if(policy.Drivers.Any(d => d.Occupation == Occupation.Chauffeur))
            {
                currentPremium += currentPremium * 0.1m;
            }

            if (policy.Drivers.Any(d => d.Occupation == Occupation.Accountant))
            {
                currentPremium -= currentPremium * 0.1m;
            }

            return currentPremium;
        }
    }
}
