using Demo.InsuranceCalculation.Data;

namespace Demo.InsuranceCalculation.PremiumCalculation
{
    /// <summary>
    /// An implementation of <see cref="IPremiumCalculationRule"/> responsible for calculating
    /// insurance premium based on the occupation of the drivers on the policy.
    /// </summary>
    public class DriverOccupationPremiumCalculationRule : IPremiumCalculationRule
    {
        public decimal CalculatePremium(InsurancePolicy policy, decimal currentPremium)
        {
            return currentPremium;
        }
    }
}
