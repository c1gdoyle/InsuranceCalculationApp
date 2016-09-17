using Demo.InsuranceCalculation.Data;
using Demo.InsuranceCalculation.Utilities;

namespace Demo.InsuranceCalculation.PolicyDeclination
{
    /// <summary>
    /// An implementation of <see cref="IPolicyDeclinationRule"/> that checks if an insurance
    /// policy application is to be declined or not based on the number of prior claims taken
    /// out by each individual driver on the policy
    /// </summary>
    public class IndividualDriverClaimsPolicyDeclinationRule : IPolicyDeclinationRule
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
        /// If a driver on the policy has more than 2 claims the policy will be declined
        /// with the message "Driver has more than 2 claims" and the name of the driver appended onto the message.
        /// </remarks>
        public InsurancePolicyApplicationResult AssessPolicy(InsurancePolicy policy)
        {
            foreach(var driver in policy.Drivers)
            {
                if(driver.Claims != null && driver.Claims.Count > 2)
                {
                    return new InsurancePolicyApplicationResult
                    {
                        IsPolicyApproved = false,
                        Message = string.Format("Driver has more than 2 claims, {0}", driver.Name)
                    };
                }
            }

            return new InsurancePolicyApplicationResult { IsPolicyApproved = true };
        }
    }
}
