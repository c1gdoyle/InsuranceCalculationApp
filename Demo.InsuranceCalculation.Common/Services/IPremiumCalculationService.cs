using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.PremiumCalculation;
using Demo.InsuranceCalculation.Utilities;

namespace Demo.InsuranceCalculation.Services
{
    /// <summary>
    /// Defines the behaviour of a service responsible for calculating an insurance policy's
    /// premium against <see cref="IPremiumCalculationRule"/>'s.
    /// </summary>
    public interface IPremiumCalculationService
    {
        /// <summary>
        /// Registers a pre-defined rule in this service.
        /// </summary>
        /// <param name="rule">The rule to be registered.</param>
        void RegisterPremiumCalculationRule(IPremiumCalculationRule rule);

        /// <summary>
        /// Calculates the total premium a specified policy will cost after 
        /// assessing it against all the rules registered in this service.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns>
        /// The total premium for the insurance policy.
        /// </returns>
        decimal CalculatePremium(InsurancePolicy policy);
    }
}
