using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.PolicyDeclination;
using Demo.InsuranceCalculation.Utilities;

namespace Demo.InsuranceCalculation.Services
{
    /// <summary>
    /// Defines the behaviour of a service responsible for assessing whether an insurance policy
    /// is approved or declined.
    /// </summary>
    public interface IPolicyDeclinationService
    {
        /// <summary>
        /// Registers a pre-defined decline rule in this service.
        /// </summary>
        /// <param name="rule">The decline rule to be registered.</param>
        void RegisterPolicyDeclinationRule(IPolicyDeclinationRule rule);

        /// <summary>
        /// Assesses whether a specified insurance policy is approved or declined by 
        /// assessing it against all the rules registered in this service.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns>
        /// /// A <see cref="InsurancePolicyApplicationResult"/> that representing whether or not
        /// the supplied policy was approved or declined.
        /// 
        /// If the policy was not declined by anything of rules registered in this service the 
        /// <see cref="InsurancePolicyApplicationResult.IsPolicyApproved"/> property will 
        /// be true.
        /// 
        /// If the policy was declined the <see cref="InsurancePolicyApplicationResult.IsPolicyApproved"/> property will
        /// be false and the <see cref="InsurancePolicyApplicationResult.Message"/> will contain the details as to
        /// why the policy was declined.
        /// </returns>
        InsurancePolicyApplicationResult AssessPolicy(InsurancePolicy policy);
    }
}
