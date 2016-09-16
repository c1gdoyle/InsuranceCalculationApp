using System.Linq;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.Extensions;

namespace Demo.InsuranceCalculation.PremiumCalculation
{
    /// <summary>
    /// An implementation of <see cref="IPremiumCalculationRule"/> responsible for calculating
    /// insurance premium based on the age of the drivers on the policy.
    /// </summary>
    public class DriverAgePremiumCalculationRule : IPremiumCalculationRule
    {
        /// <summary>
        /// Gets the order in which this rule is to be applied
        /// when calculating the insurance premium.
        /// 
        /// In this case 3.
        /// </summary>
        public int OrderOfPrecedence
        {
            get { return 3; }
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
        /// - If the youngest driver is aged between 21 and 25 at the start date of the
        /// policy increase the premium by 20%
        /// - If the youngest driver is aged between 26 and 75 at the start date of the
        /// policy decrease the premium by 20%
        /// </remarks>
        public decimal CalculatePremium(InsurancePolicy policy, decimal currentPremium)
        {
            //get the youngest driver
            var youngestDriver = policy.Drivers.OrderByDescending(d => d.DateOfBirth).FirstOrDefault();

            //get their age at the start of the policy
            int age = youngestDriver.DateOfBirth.GetAgeAtDate(policy.StartDate);

            if(age >= 21 && age <= 25)
            {
                currentPremium += currentPremium * 0.2m;
            }
            else if(age >= 26 && age <= 75)
            {
                currentPremium -= currentPremium * 0.1m;
            }

            return currentPremium;
        }
    }
}
