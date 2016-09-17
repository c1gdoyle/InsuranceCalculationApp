using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.Utilities;

namespace Demo.InsuranceCalculation.PolicyDeclination
{
    /// <summary>
    /// Defines the behaviour of a class that represents a rule for 
    /// whether an insurance policy application is declined or not.
    /// </summary>
    public interface IPolicyDeclinationRule
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
        InsurancePolicyApplicationResult AssessPolicy(InsurancePolicy policy);
    }
}
