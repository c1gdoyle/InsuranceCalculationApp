using System;
using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.Utilities;

namespace Demo.InsuranceCalculation.PolicyDeclination
{
    /// <summary>
    /// An implementation of <see cref="IPolicyDeclinationRule"/> that checks if an insurance
    /// policy application is to be declined or not based on the Start Date of the policy. 
    /// </summary>
    public class StartDatePolicyDeclinationRule : IPolicyDeclinationRule
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
        /// If that start date of the policy is before today the policy will be declined with the message
        /// "Start Date Of Policy".
        /// </remarks>
        public InsurancePolicyApplicationResult AssessPolicy(InsurancePolicy policy)
        {
            if(policy.StartDate < DateTime.Today)
            {
                return new InsurancePolicyApplicationResult { IsPolicyApproved = false, Message = "Start Date of Policy" };
            }

            return new InsurancePolicyApplicationResult { IsPolicyApproved = true };
        }
    }
}
