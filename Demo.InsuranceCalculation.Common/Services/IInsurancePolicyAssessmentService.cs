using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.Utilities;

namespace Demo.InsuranceCalculation.Services
{
    /// <summary>
    /// Defines behaviour of a service responsible for assessing an <see cref="InsurancePolicy"/> application.
    /// </summary>
    public interface IInsurancePolicyAssessmentService
    {
        /// <summary>
        /// Assesses a given insurance policy and determines whether the policy is approved or declined. If
        /// approved will also calculate the final premium.
        /// </summary>
        /// <param name="policy">The insurance policy to be assessed.</param>
        /// <returns>
        /// A <see cref="InsurancePolicyApplicationResult"/> instance representing whether the insurance policy
        /// was approved or declined.
        /// 
        /// If the policy was approved the <see cref="InsurancePolicyApplicationResult.IsPolicyApproved"/> property will be
        /// true and the <see cref="InsurancePolicyApplicationResult.PremiumAmount"/> property will contain the final calculated premium
        /// for the policy.
        /// 
        /// If the policy was declined the <see cref="InsurancePolicyApplicationResult.IsPolicyApproved"/> property will be
        /// false and the <see cref="InsurancePolicyApplicationResult.Message"/> property will contain the reason why the policy
        /// was declined.
        /// 
        /// </returns>
        InsurancePolicyApplicationResult AssessPolicyApplication(InsurancePolicy policy);
    }
}
