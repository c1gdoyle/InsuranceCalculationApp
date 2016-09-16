using Demo.InsuranceCalculation.Data;

namespace Demo.InsuranceCalculation.PremiumCalculation
{
    /// <summary>
    /// An implementation of <see cref="IPremiumCalculationRule"/> responsible for calculating
    /// insurance premium based on any claims taken out by the the drivers on the policy.
    /// </summary>
    public class DriverClaimsPremiumCalculationRule : IPremiumCalculationRule
    {
        /// <summary>
        /// Gets the order in which this rule is to be applied
        /// when calculating the insurance premium.
        /// 
        /// In this case 4.
        /// </summary>
        public int OrderOfPrecedence
        {
            get { return 4; }
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
        /// - For each claim within 1 year of the start date of the policy increase the 
        /// premium by 20 %.
        /// - For each claim within 2 - 5 years of the start date of the policy increase the
        /// premium by 10 %.
        /// </remarks>
        public decimal CalculatePremium(InsurancePolicy policy, decimal currentPremium)
        {
            var claims = policy.PriorClaims;

            foreach(Claim claim in claims)
            {
                if(policy.StartDate.AddYears(-1) < claim.DateOfClaim)
                {
                    currentPremium += currentPremium * 0.2m;
                }
                else if(policy.StartDate.AddYears(-5) < claim.DateOfClaim)
                {
                    currentPremium += currentPremium * 0.1m;
                }
            }

            return currentPremium;
        }
    }
}
