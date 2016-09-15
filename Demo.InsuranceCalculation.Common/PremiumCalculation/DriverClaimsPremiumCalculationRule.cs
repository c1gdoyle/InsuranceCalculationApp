using Demo.InsuranceCalculation.Data;

namespace Demo.InsuranceCalculation.PremiumCalculation
{
    /// <summary>
    /// An implementation of <see cref="IPremiumCalculationRule"/> responsible for calculating
    /// insurance premium based on any claims taken out by the the drivers on the policy.
    /// </summary>
    public class DriverClaimsPremiumCalculationRule : IPremiumCalculationRule
    {
        public decimal CalculatePremium(InsurancePolicy policy, decimal currentPremium)
        {
            return currentPremium;
        }
    }
}
