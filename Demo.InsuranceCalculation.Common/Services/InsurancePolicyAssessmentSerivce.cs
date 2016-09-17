using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.Utilities;

namespace Demo.InsuranceCalculation.Services
{
    /// <summary>
    /// An implementation of <see cref="IInsurancePolicyAssessmentService"/>.
    /// </summary>
    public class InsurancePolicyAssessmentSerivce : IInsurancePolicyAssessmentService
    {
        private readonly IPremiumCalculationService _premiumCalculuationService;
        private readonly IPolicyDeclinationService _policyDeclinationService;

        /// <summary>
        /// Initialises a new instance of <see cref="IInsurancePolicyAssessmentService"/> with a specified
        /// premium calculation and policy declination service.
        /// </summary>
        /// <param name="premiumCalculation">The premium calculation service with all calculation rules already registered.</param>
        /// <param name="policyDeclination">The policy declination service with all decline rules already registered.</param>
        public InsurancePolicyAssessmentSerivce(IPremiumCalculationService premiumCalculation, IPolicyDeclinationService policyDeclination)
        {
            _premiumCalculuationService = premiumCalculation;
            _policyDeclinationService = policyDeclination;
        }

        #region IInsuranePolicyAssessmentService Members
        public InsurancePolicyApplicationResult AssessPolicyApplication(InsurancePolicy policy)
        {
            //first off check if policy is declined
            var result = _policyDeclinationService.AssessPolicy(policy);
            if(!result.IsPolicyApproved)
            {
                return result;
            }

            //policy is approved, now calculate premium
            decimal premium = _premiumCalculuationService.CalculatePremium(policy);
            result.PremiumAmount = premium;
            return result;
            
        }
        #endregion IInsuranePolicyAssessmentService Members
    }
}
