using System.Linq;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.Utilities;

namespace Demo.InsuranceCalculation.PolicyDeclination
{
    /// <summary>
    /// An implementation of <see cref="IPolicyDeclinationRule"/> that checks if an insurance
    /// policy application is to be declined or not based on the total number of prior claims 
    /// on the policy.
    /// </summary>
    public class TotalClaimsPolicyDeclinationRule : IPolicyDeclinationRule
    {
        /// <summary>
        /// Assesses whether a given a insurance policy should be accepted or
        /// declined based on this rule.
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>
        /// A <see cref="InsurancePolicyApplicationResult"/> that representing whether or not
        /// the supplied policy was declined based on this rule.
        /// 
        /// If the policy was not declined the <see cref="InsurancePolicyApplicationResult.IsPolicyApproved"/> property will 
        /// be true.
        /// 
        /// If the policy was declined the <see cref="InsurancePolicyApplicationResult.IsPolicyApproved"/> property will
        /// be false and the <see cref="InsurancePolicyApplicationResult.Message"/> will contain the details as to
        /// why the policy was declined.
        /// </returns>
        /// <remarks>
        /// If the total number of claims exceeds 3 then the policy will be declined with the message 
        /// "Policy has more than 3 claims".
        /// </remarks>
        public InsurancePolicyApplicationResult AssessPolicy(InsurancePolicy policy)
        {
            if(policy.PriorClaims.Count() > 3)
            {
                return new InsurancePolicyApplicationResult
                {
                    IsPolicyApproved = false,
                    Message = "Policy has more than 3 claims"
                };
            }

            return new InsurancePolicyApplicationResult { IsPolicyApproved = true };
        }
    }
}
