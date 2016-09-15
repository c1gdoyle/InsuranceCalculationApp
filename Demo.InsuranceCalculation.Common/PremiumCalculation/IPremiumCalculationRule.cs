using Demo.InsuranceCalculation.Data;

namespace Demo.InsuranceCalculation.PremiumCalculation
{
    /// <summary>
    /// Defines the behaviour of a policy pattern that represents a rule for 
    /// calculates insurance premium.
    /// </summary>
    public interface IPremiumCalculationRule
    {
        /// <summary>
        /// Calculates the cost of an insurance premium for a given insurance policy.
        /// </summary>
        /// <param name="policy">The insurance policy.</param>
        /// <param name="currentPremium">The current premium amount.</param>
        /// <returns>
        /// The cost of the insurance premium after assessing the policy against 
        /// this rule.
        /// </returns>
        decimal CalculatePremium(InsurancePolicy policy, decimal currentPremium);
    }
}
